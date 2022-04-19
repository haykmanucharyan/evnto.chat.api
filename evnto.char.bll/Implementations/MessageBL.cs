using evnto.chat.bll.Interfaces;
using evnto.chat.dal;
using evnto.chat.dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.bll.Implementations
{
    internal class MessageBL : BaseBL, IMessageBL
    {
        internal MessageBL(BLConfiguration config) : base(config)
        {
            
        }

        public void CreteMessage(int userId, int chatId, string text)
        {
            if(!string.IsNullOrEmpty(text))
                throw new ArgumentNullException(Errors.EmptyText);

            using (EvntoChatDBContext context = CreateDbContext())
            {
                Chat chat = context.Chats.FirstOrDefault(cht => cht.ChatId == chatId
                                                        && cht.State == (byte)ChatState.Accepted
                                                        && (cht.InitiatorUserId == userId || cht.RecipientUserId == userId));

                if (chat == null)
                    throw new Exception(Errors.ChatMissing);

                Message message = new Message();
                message.Created = DateTimeOffset.Now;
                message.ChatId = chatId;
                message.AuthorUserId = userId;
                message.Text = text;

                context.Messages.Add(message);
                context.SaveChanges();
            }
        }

        public List<Message> GetChatMessages(int userId, int chatId)
        {
            using (EvntoChatDBContext context = CreateDbContext())
            {
                Chat chat = context.Chats.FirstOrDefault(cht => cht.ChatId == chatId
                                                        && cht.State == (byte)ChatState.Accepted
                                                        && (cht.InitiatorUserId == userId || cht.RecipientUserId == userId));

                if (chat == null)
                    throw new Exception(Errors.ChatMissing);

                return context.Messages.Include(m => m.AuthorUser).Where(m => m.ChatId == chatId).ToList();
            }
        }
    }
}
