using evnto.chat.bus;

namespace evnto.chat.bll.Interfaces
{
    public interface IBLFactory
    {
        BLConfiguration Configuration { get; }

        IUserBL CreateUserBL();

        IChatBL CreateChatBL();

        IMessageBL CreateMessageBL();

        IRmqConnector GetRmqConnector();
    }
}
