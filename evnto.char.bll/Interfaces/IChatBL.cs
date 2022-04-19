using evnto.chat.dal.Entities;

namespace evnto.chat.bll.Interfaces
{
    public interface IChatBL
    {
        void InitiateChat(int initiatorUserId, int recipientUserId);

        void AcceptOrRejectChat(int chatId, int recipientUserId, bool accpept);

        void CloseChat(int chatId, int userId);

        List<Chat> GetActiveChats(int userId);
    }
}
