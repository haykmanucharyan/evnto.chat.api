namespace evnto.chat.bll.Interfaces
{
    public interface IBLFactory
    {
        IUserBL CreateUserBL();

        IChatBL CreateChatBL();

        IMessageBL CreateMessageBL();
    }
}
