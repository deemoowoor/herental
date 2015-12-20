using herental.BL;
using herental.BL.Interfaces;
using log4net;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SimpleInjector;
using System;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace herental.backend
{
    public partial class HerentalBackendService : ServiceBase
    {
        protected Thread MainThread;
        private readonly ILog Log = LogManager.GetLogger(typeof(HerentalBackendService));

        AutoResetEvent StopRequest = new AutoResetEvent(false);
        Container container;

        public HerentalBackendService()
        {
            InitializeComponent();

            this.ServiceName = "Herental Backend Service";
            this.EventLog.Log = "HerentalBackend";

            this.CanHandlePowerEvent = false;
            this.CanHandleSessionChangeEvent = false;
            this.CanPauseAndContinue = false;
            this.CanShutdown = true;
            this.CanStop = true;

            MainThread = new Thread(MainLoop);

            container = DependencyInjectorSetup.Setup();
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            HerentalBL.WarmUp();
            HerentalBL.SeedWithTestData();
            MainThread.Start();
        }

        protected override void OnStop()
        {
            try {
                StopRequest.Set();
                MainThread.Join(3000);
            }
            finally
            {
                StopRequest.Dispose();
            }
            base.OnStop();
        }

        protected void MainLoop()
        {
            var cf = new ConnectionFactory() { HostName = "localhost" };
            
            // set the heartbeat timeout to 60 seconds
            cf.RequestedHeartbeat = 60;

            var dispatcher = container.GetInstance<IDispatcher>();

            // TODO: implement a single-queue RPC server
            using (var connection = cf.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                
                channel.QueueDeclare(queue: "rpc_queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                                     noAck: false,
                                     consumer: consumer);

                for (;;)
                {
                    PollQueue(channel, consumer, dispatcher);

                    if (StopRequest.WaitOne(100))
                        return;
                }
            }
        }

        protected void PollQueue(IModel channel, IQueueingBasicConsumer consumer, IDispatcher dispatcher)
        {
            string response = null;
            try
            {
                var ea = consumer.Queue.Dequeue();
                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    Log.InfoFormat("recv: '{0}'", message);
                    var rawcommand = JsonConvert.DeserializeObject<ProtocolRequest>(message);
                    var result = dispatcher.Invoke(rawcommand.MethodName, rawcommand.Arguments);
                    response = JsonConvert.SerializeObject(new ProtocolResponse(result)); // TODO: wrap into a protocol message object
                    Log.InfoFormat("send: '{0}'", response);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    response = JsonConvert.SerializeObject(new { Error = e.Message}); // TODO: report an error
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish(exchange: "",
                                         routingKey: props.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                    channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                     multiple: false);
                }

            }
            catch (EndOfStreamException)
            {
                return;
            }

        }

    }
}
