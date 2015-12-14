using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.ServiceProcess;
using System.Text;

namespace herental.backend.Tests
{

    class BasicRabbitMqClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;
        private TimeSpan timeout;

        public BasicRabbitMqClient(TimeSpan timeout)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                noAck: true,
                consumer: consumer);
            this.timeout = timeout;
        }

        public string Call(string message)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                 routingKey: "rpc_queue",
                                 basicProperties: props,
                                 body: messageBytes);

            DateTime start = DateTime.UtcNow;

            while (true)
            {
                if (DateTime.UtcNow > (start + this.timeout)) { 
                    throw new System.TimeoutException("Timed out waiting for reply!");
                }

                var ea = consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }
        }

        public void Close()
        {
            connection.Close();
        }
    }

    [TestClass]
    public class BackendServiceTest
    {
        private static TimeSpan timeout = TimeSpan.FromMilliseconds(3000);
        private static ServiceController service = new ServiceController("herental.backend");

        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            // XXX: service must be installed prior to running this test!
            ServiceController service = new ServiceController("herental.backend");
            
            if (ServiceControllerStatus.Stopped == service.Status)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }

            Assert.AreEqual(ServiceControllerStatus.Running, service.Status);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            if (ServiceControllerStatus.Running == service.Status)
            {
                service.Stop();
            }

            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            Assert.AreEqual(ServiceControllerStatus.Stopped, service.Status);
        }

        [TestMethod]
        public void TestRPCFunction()
        {
            var rpcClient = new BasicRabbitMqClient(timeout);
            var result = rpcClient.Call("30");
            rpcClient.Close();
            Assert.AreEqual("30 ack!", result);
        }
    }
}