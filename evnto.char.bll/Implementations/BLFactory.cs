using evnto.chat.bll.Interfaces;

namespace evnto.chat.bll.Implementations
{
    public class BLFactory : IBLFactory
    {
        private BLConfiguration _config;

        public BLFactory(BLConfiguration config)
        { 
            _config = config;
        }

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
    }
}
