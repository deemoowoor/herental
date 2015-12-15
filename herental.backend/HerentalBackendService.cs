using System;
using System.ServiceProcess;
using System.Text;
using log4net;
using System.Threading;
using RabbitMQ.Client;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace herental.backend
{
    public partial class HerentalBackendService : ServiceBase
    {
        protected Thread MainThread;
        private readonly ILog Log = LogManager.GetLogger(typeof(HerentalBackendService));

        AutoResetEvent StopRequest = new AutoResetEvent(false);

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
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            MainThread.Start();
        }

        protected override void OnStop()
        {
            StopRequest.Set();
            MainThread.Join(3000);
            base.OnStop();
        }

        protected void MainLoop()
        {
            // TODO: receive events from the MQ, act upon the messages
            // TODO: get the hostname from configuration file
            var cf = new ConnectionFactory() { HostName = "localhost" };
            
            // set the heartbeat timeout to 60 seconds
            // TODO: config
            cf.RequestedHeartbeat = 60;

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
                    string response = null;
                    try {
                        var ea = consumer.Queue.Dequeue();
                        var body = ea.Body;
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            Log.InfoFormat("Recv: '{0}'", message);

                            response = String.Format("{0} ack!", message);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                            response = "";
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

                    if (StopRequest.WaitOne(100))
                        return;
                }
            }
        }

    }
}
