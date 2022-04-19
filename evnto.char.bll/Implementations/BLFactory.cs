using evnto.chat.bll.Interfaces;
using evnto.chat.bus;

namespace evnto.chat.bll.Implementations
{
    public class BLFactory : IBLFactory
    {
        #region Fields

        private BLConfiguration _config;
        private static IRmqConnector _rmqConnectorInstance = null;
        private static object _syncRoot = new object();

        #endregion

        #region Ctor

        public BLFactory(BLConfiguration config)
        {
            _config = config;
        }

        #endregion

        #region Interface implementation

        public IChatBL CreateChatBL()
        {
            return new ChatBL(_config);
        }

        public IUserBL CreateUserBL()
        {
            return new UserBL(_config);
        }

        public IMessageBL CreateMessageBL()
        {
            return new MessageBL(_config);
        }

        public IRmqConnector GetRmqConnector()
        {
            if (_rmqConnectorInstance == null)
                lock (_syncRoot)
                    if (_rmqConnectorInstance == null)
                        _rmqConnectorInstance = new RmqConnector(_config.ApiKey, _config.RMQHost, _config.RMQPort, _config.RMQUser, _config.RMQPassword);

            return _rmqConnectorInstance;
        }

        #endregion
    }
}
