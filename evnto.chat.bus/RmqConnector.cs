using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace evnto.chat.bus
{
    public class RmqConnector : IRmqConnector
    {
        public delegate void RmqMessageHandler(byte[] message);

        public event RmqMessageHandler RmqMessageArrived;

        ConnectionFactory _factory;
        IConnection _connection;
        IModel _channel;
        string _apiKey;

        const string _exchangeGlobal = "evnto.chat.global";
        const string _exchangeRouted = "evnto.chat.routed";

        const string _queueGlobal = "evnto.chat.global";
        const string _queueRouted = "evnto.chat.routed_";
        string _routedQueueName;

        const int _maxLength = 8192; // 8KB
        const int _ttl = 30_000; // 30 seconds
        const int _expires = 1_800_000; // 30 minutes

        public RmqConnector(string apiKey, string host, int port, string username, string password)
        {
            _apiKey = apiKey;

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

        public void ConnectAndInit()
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchangeGlobal, "direct", true, false);
            _channel.ExchangeDeclare(_exchangeRouted, "fanout", true, false);

            _channel.QueueDeclare(_queueGlobal, true, false, false, 
                new Dictionary<string, object>() 
                {
                    { "x-max-length", _maxLength }, 
                    { "x-message-ttl", _ttl }, 
                    { "x-expires", _expires } 
                });

            _routedQueueName = _queueRouted + _apiKey;

            _channel.QueueDeclare(_routedQueueName, true, false, true, // auto delete routed one
                new Dictionary<string, object>()
                {
                    { "x-max-length", _maxLength },
                    { "x-message-ttl", _ttl },
                    { "x-expires", _expires }
                });

            _channel.QueueBind(_queueGlobal, _exchangeGlobal, "*");
            _channel.QueueBind(_routedQueueName, _exchangeRouted, _apiKey);
        }

        public void PublishGlobal(byte[] message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            _channel.BasicPublish(_exchangeGlobal, "*", properties, message);
        }

        public void PublishRouted(byte[] message)
        {
            var properties = _channel.CreateBasicProperties();
            properties.DeliveryMode = 2;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            _channel.BasicPublish(_exchangeRouted, _apiKey, properties, message);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            _channel = null;
            _connection = null;
        }

        public void BeginConsume()
        {
            var consumerGlobal = new EventingBasicConsumer(_channel);
            consumerGlobal.Received += Consumer_Received;

            var consumerRouted = new EventingBasicConsumer(_channel);
            consumerRouted.Received += Consumer_Received;

            _channel.BasicConsume(_queueGlobal, true, consumerGlobal);
            _channel.BasicConsume(_routedQueueName, true, consumerRouted);
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            EventingBasicConsumer consumer = sender as EventingBasicConsumer;

            consumer.Model.BasicAck(e.DeliveryTag, false);

            RmqMessageArrived?.Invoke(e.Body.ToArray());
        }
    }
}
