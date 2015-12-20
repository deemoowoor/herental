using herental.Interfaces;
using Newtonsoft.Json;
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

        public class RequestMessage
        {
            public string MethodName { get; set; }
            public object[] Arguments { get; set; }
        }

        [JsonObject()]
        public class ResponseMessage<TObject>
        {
            [JsonProperty(PropertyName = "Status")]
            public string Status { get; set; }

            [JsonProperty(PropertyName = "Result")]
            public TObject Result { get; set; }
        }

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

        public virtual Guid SendMessage(byte[] message)
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

        public virtual byte[] WaitForResponse(Guid corrId)
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

        public virtual TObject Call<TObject>(string methodName, object[] args)
        {
            var reqMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                new RequestMessage { MethodName = methodName, Arguments = args }));
            var corrId = SendMessage(reqMessage);
            var response = Encoding.UTF8.GetString(WaitForResponse(corrId));

            JsonSerializerSettings settings = new JsonSerializerSettings();
            /*
            // TODO: find a better place for this
            List<string> errorList = new List<string>();
            settings.Error += delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs eargs) {
                errorList.Add(eargs.ErrorContext.Error.Message);
            };

            if (errorList.Count > 0)
            {
                throw new Exception(String.Join("\n", errorList));
            }
            */
            var result = JsonConvert.DeserializeObject<ResponseMessage<TObject>>(response);
            return result.Result;
        }
        
        public virtual void Close()
        {
            connection.Close();
        }
    }
}