using evnto.chat.bll.Interfaces;

namespace evnto.chat.bll.Implementations
{
    public class BLFactory : IBLFactory
    {
        #region Fields

        private BLConfiguration _config;

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

        #endregion
    }
}
