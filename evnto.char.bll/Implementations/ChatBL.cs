using evnto.chat.bll.Interfaces;
using evnto.chat.dal;
using evnto.chat.dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.bll.Implementations
{
    internal class ChatBL : BaseBL, IChatBL
    {
        internal ChatBL(BLConfiguration config) : base(config)
        {
        }

        public void InitiateChat(int initiatorUserId, int recipientUserId)
        {
            Chat chat = new Chat();
            chat.InitiatorUserId = initiatorUserId;
            chat.RecipientUserId = recipientUserId;
            chat.State = (byte)ChatState.Initiated;
            chat.Created = DateTimeOffset.Now;

            using (EvntoChatDBContext context = CreateDbContext())
            {
                context.Chats.Add(chat);
                context.SaveChanges();
            }
        }

        public void AcceptOrRejectChat(int chatId, int recipientUserId, bool accpept)
        {
            using (EvntoChatDBContext context = CreateDbContext())
            {
                Chat chat = context.Chats.FirstOrDefault(cht => cht.ChatId == chatId
                                                                && cht.RecipientUserId == recipientUserId
                                                                && cht.State == (byte)ChatState.Initiated);

                if (chat == null)
                    throw new Exception(Errors.ChatMissing);

                chat.State = accpept ? (byte)ChatState.Accepted : (byte)ChatState.Rejected;
                context.Update(chat);

                context.SaveChanges();
            }
        }

        public void CloseChat(int chatId, int userId)
        {
            using (EvntoChatDBContext context = CreateDbContext())
            {
                Chat chat = context.Chats.FirstOrDefault(cht => cht.ChatId == chatId
                                                        && cht.State == (byte)ChatState.Accepted
                                                        && (cht.InitiatorUserId == userId || cht.RecipientUserId == userId));

                if (chat == null)
                    throw new Exception(Errors.ChatMissing);

                chat.State = (byte)ChatState.Closed;
                context.Update(chat);

                context.SaveChanges();
            }
        }

        public List<Chat> GetActiveChats(int userId)
        {
            using (EvntoChatDBContext context = CreateDbContext())
            {
                return (from c in context.Chats.Include(c => c.RecipientUser).Include(c => c.InitiatorUser)
                        where (c.InitiatorUserId == userId || c.RecipientUserId == userId)
                        && (c.State == (byte)ChatState.Initiated || c.State == (byte)ChatState.Accepted)
                        select c).ToList();
            }
        }
    }
}
