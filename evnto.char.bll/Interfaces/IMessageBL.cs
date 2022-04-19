using evnto.chat.dal.Entities;

namespace evnto.chat.bll.Interfaces
{
    public interface IMessageBL
    {
        void CreteMessage(int userId, int chatId, string text);

        List<Message> GetChatMessages(int userId, int chatId);
    }
}
