using evnto.chat.bll.Interfaces;
using evnto.chat.bus;
using evnto.chat.dal;
using evnto.chat.dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.bll.Implementations
{
    internal class MessageBL : BaseBL, IMessageBL
    {
        internal MessageBL(IBLFactory factory) : base(factory)
        {
            
        }

        public void CreteMessage(int userId, int chatId, string text)
        {
            if(string.IsNullOrEmpty(text))
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

                message = context.Messages.Include(m => m.AuthorUser).FirstOrDefault(m => m.MessageId == message.MessageId);

                // rmq publish if initiator user or/and recipient user online
                IUserBL bl = BLFactory.CreateUserBL();
                UserSession session1 = bl.GetSessionByUser(chat.RecipientUserId);
                UserSession session2 = bl.GetSessionByUser(chat.InitiatorUserId);

                if (session1 != null || session2 != null)
                {
                    RmqMessage rmqMessage = new RmqMessage(RmqMessageType.Message);
                    rmqMessage.PayLoad.Add(nameof(Message.MessageId), message.MessageId.ToString());
                    rmqMessage.PayLoad.Add(nameof(Message.Created), message.Created.UtcTicks.ToString());
                    rmqMessage.PayLoad.Add(nameof(Message.ChatId), message.ChatId.ToString());
                    rmqMessage.PayLoad.Add(nameof(Message.AuthorUserId), message.AuthorUserId.ToString());
                    rmqMessage.PayLoad.Add(nameof(Message.Text), message.Text);

                    rmqMessage.PayLoad.Add($"{nameof(Message.AuthorUser)}.{nameof(User.UserId)}", message.AuthorUser.ToString());
                    rmqMessage.PayLoad.Add($"{nameof(Message.AuthorUser)}.{nameof(User.UserName)}", message.AuthorUser.UserName);
                    rmqMessage.PayLoad.Add($"{nameof(Message.AuthorUser)}.{nameof(User.FullName)}", message.AuthorUser.FullName);

                    if (session1 != null)
                        BLFactory.GetRmqConnector().PublishRouted(session1.ApiKey, rmqMessage);

                    if (session2 != null)
                        BLFactory.GetRmqConnector().PublishRouted(session2.ApiKey, rmqMessage);
                }
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
