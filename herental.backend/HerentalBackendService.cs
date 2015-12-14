using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

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
            MainThread.Join();
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
                channel.QueueDeclare(queue: "herental.backend",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;

                for (;;)
                {
                    if (StopRequest.WaitOne(100)) return;
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            Log.InfoFormat("Received a message: '{0}'", message);
            // TODO: handle the message according to its logic
        }
    }
}
