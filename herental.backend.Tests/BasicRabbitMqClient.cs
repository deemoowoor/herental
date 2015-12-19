using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using System;
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

        public string Call(byte[] message)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = message;
            channel.BasicPublish(exchange: "",
                                 routingKey: "rpc_queue",
                                 basicProperties: props,
                                 body: messageBytes);

            DateTime start = DateTime.UtcNow;

            while (true)
            {
                if (DateTime.UtcNow > (start + this.timeout))
                {
                    throw new TimeoutException("Timed out waiting for reply!");
                }

                var ea = consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }
        }

        public object Call(string methodName, object[] args)
        {
            var response = Call(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { MethodName = "ListProducts", Arguments = new object[1] })));
            var result = ((JObject)JsonConvert.DeserializeObject(response))["Result"];
            return result;
        }

        public void Close()
        {
            connection.Close();
        }
    }

}
