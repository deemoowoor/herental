using herental.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
using System.Text;

namespace herental.Services
{
    public class RpcClient : IRpcClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public TimeSpan Timeout { get; set; }

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                noAck: true,
                consumer: consumer);

            Timeout = TimeSpan.FromSeconds(3);
        }

        public Guid SendMessage(byte[] message)
        {
            var corrId = Guid.NewGuid();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId.ToString();

            var messageBytes = message;
            channel.BasicPublish(exchange: "",
                                 routingKey: "rpc_queue",
                                 basicProperties: props,
                                 body: messageBytes);
            return corrId;
        }

        public byte[] WaitForResponse(Guid corrId)
        {
            DateTime start = DateTime.UtcNow;

            while (true)
            {
                if (DateTime.UtcNow > (start + Timeout))
                {
                    throw new TimeoutException("Timed out waiting for reply!");
                }

                var ea = consumer.Queue.Dequeue();
                // XXX: will discard all messages that do not match corrId
                if (ea.BasicProperties.CorrelationId == corrId.ToString())
                {
                    return ea.Body;
                }
            }
        }

        public object Call(string methodName, object[] args)
        {
            var reqMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                new { MethodName = methodName, Arguments = args }));
            var corrId = SendMessage(reqMessage);
            var response = Encoding.UTF8.GetString(WaitForResponse(corrId));
            var result = ((JObject)JsonConvert.DeserializeObject(response))["Result"];
            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }
}