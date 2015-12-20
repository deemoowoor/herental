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
            public const string OK = "OK";
            public const string ERR = "ERR";

            [JsonProperty(PropertyName = "Status")]
            public string Status { get; set; }

            [JsonProperty(PropertyName = "Result")]
            public TObject Result { get; set; }

            // Reserved for error messages
            [JsonProperty(PropertyName = "Message")]
            public string Message { get; set; }
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

        /// <summary>
        /// Perform a synchronous "RPC" call
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <exception cref="RemoteError">Thrown when backend server responds with an error</exception>
        /// <returns></returns>
        public virtual TObject Call<TObject>(string methodName, object[] args)
        {
            var reqMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
                new RequestMessage() { MethodName = methodName, Arguments = args }));
            var corrId = SendMessage(reqMessage);
            var response = Encoding.UTF8.GetString(WaitForResponse(corrId));
            var result = JsonConvert.DeserializeObject<ResponseMessage<TObject>>(response);
            if (result.Status == ResponseMessage<TObject>.ERR)
            {
                throw new RemoteError(result.Message);
            }

            return result.Result;
        }
        
        public virtual void Close()
        {
            connection.Close();
        }
    }

    public class RemoteError : Exception
    {
        public RemoteError(string message) : base(message) { }
    }
}