using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace evnto.chat.bus
{
    public class RmqConnector : IRmqConnector
    {
        public delegate void RmqMessageHandler(bool isGlobal, RmqMessage message);

        public event RmqMessageHandler RmqMessageArrived;

        ConnectionFactory _factory;
        IConnection _connection;
        IModel _channelConsumer;
        string _apiKey;

        const string _exchangeName = "evnto.chat.exchange";
        const string _queueRouted = "evnto.chat.routed_";

        string _routedQueueName;

        Dictionary<string, object> _queueArgs = new Dictionary<string, object>()
                                                    {
                                                        { "x-max-length", 8192 }, // 8KB
                                                        { "x-message-ttl", 30_000 }, // 30 seconds
                                                        { "x-expires", 108_000_000 } // 30 minutes
                                                    };

        public RmqConnector(string apiKey, string host, int port, string username, string password)
        {
            _apiKey = apiKey;
            _routedQueueName = _queueRouted + _apiKey;

            _factory = new ConnectionFactory()
            {
                HostName = host,
                Port = port,
                UserName = username,
                Password = password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
            };
        }

        private bool Connect()
        {
            try
            {
                if (_connection == null || !_connection.IsOpen)
                    _connection = _factory.CreateConnection();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void PublishInternal(RmqMessage message, string routeKey)
        {
            if (!Connect())
                throw new Exception("RMQ connection failed.");

            using (IModel channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(_exchangeName, "fanout", true, false);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;
                properties.ContentType = "application/json";
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

                channel.BasicPublish(_exchangeName, routeKey, properties, message.ToBytes());
            }
        }

        public void PublishGlobal(RmqMessage message)
        {
            PublishInternal(message, "*");
        }

        public void PublishRouted(string routeKey, RmqMessage message)
        {
            PublishInternal(message, routeKey);
        }

        public void Dispose()
        {
            _channelConsumer?.Dispose();
            _channelConsumer = null;
            _connection?.Dispose();
            _connection = null;
        }

        public void BeginConsume()
        {
            if (!Connect())
                throw new Exception("RMQ connection failed.");

            _channelConsumer = _connection.CreateModel();
            _channelConsumer.ExchangeDeclare(_exchangeName, "fanout", true, false);

            _channelConsumer.QueueDeclare(_routedQueueName, true, false, true, _queueArgs); // auto delete queue

            _channelConsumer.QueueBind(_routedQueueName, _exchangeName, _apiKey);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channelConsumer);
            consumer.Received += Consumer_Received;

            _channelConsumer.BasicConsume(_routedQueueName, true, consumer);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            EventingBasicConsumer consumer = sender as EventingBasicConsumer;

            RmqMessageArrived?.Invoke(e.RoutingKey == "*", RmqMessage.FromBytes(e.Body.ToArray()));
        }
    }
}
