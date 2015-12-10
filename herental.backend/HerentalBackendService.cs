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
        protected Thread _mainLoopThread;
        private readonly ILog log = LogManager.GetLogger(typeof(HerentalBackendService));

        private bool _continue = true;

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

            _mainLoopThread = new Thread(MainLoop);
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _mainLoopThread.Start();
        }

        protected override void OnStop()
        {
            _continue = false;
            _mainLoopThread.Join();
            base.OnStop();
        }

        protected void MainLoop()
        {
            while (_continue)
            {
                try
                {
                    // TODO: receive events from the MQ, act upon the messages
                    // TODO: get the hostname from configuration file
                    var factory = new ConnectionFactory() { HostName = "localhost"  };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "herental.backend",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += Consumer_Received;
                    }
                }
                catch (System.TimeoutException)
                {

                }
                catch (Exception e)
                {
                    log.Error(e);
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            log.InfoFormat("Received a message: '{0}'", message);
        }
    }
}
