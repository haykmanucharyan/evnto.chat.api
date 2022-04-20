using evnto.chat.bll.Interfaces;
using evnto.chat.bus;
using evnto.chat.dal;
using evnto.chat.dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.bll.Implementations
{
    internal class ChatBL : BaseBL, IChatBL
    {
        internal ChatBL(IBLFactory factory) : base(factory)
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

                chat = context.Chats.Include(c => c.InitiatorUser).Include(c => c.RecipientUser).FirstOrDefault(c => c.ChatId == chat.ChatId);

                // rmq publish if initiator user or/and recipient user online
                IUserBL bl = BLFactory.CreateUserBL();
                UserSession session1 = bl.GetSessionByUser(chat.RecipientUserId);
                UserSession session2 = bl.GetSessionByUser(chat.InitiatorUserId);

                if (session1 != null || session2 != null)
                {
                    RmqMessage rmqMessage = new RmqMessage(RmqMessageType.ChatCreated);
                    rmqMessage.PayLoad.Add(nameof(Chat.ChatId), chat.ChatId.ToString());
                    rmqMessage.PayLoad.Add(nameof(Chat.Created), chat.Created.UtcTicks.ToString());
                    rmqMessage.PayLoad.Add(nameof(Chat.State), chat.State.ToString());
                    rmqMessage.PayLoad.Add(nameof(Chat.InitiatorUserId), chat.InitiatorUserId.ToString());
                    rmqMessage.PayLoad.Add(nameof(Chat.RecipientUserId), chat.RecipientUserId.ToString());

                    rmqMessage.PayLoad.Add($"{nameof(Chat.InitiatorUser)}.{nameof(User.UserId)}", chat.InitiatorUserId.ToString());
                    rmqMessage.PayLoad.Add($"{nameof(Chat.InitiatorUser)}.{nameof(User.UserName)}", chat.InitiatorUser.UserName);
                    rmqMessage.PayLoad.Add($"{nameof(Chat.InitiatorUser)}.{nameof(User.FullName)}", chat.InitiatorUser.FullName);

                    rmqMessage.PayLoad.Add($"{nameof(Chat.RecipientUser)}.{nameof(User.UserId)}", chat.RecipientUser.ToString());
                    rmqMessage.PayLoad.Add($"{nameof(Chat.RecipientUser)}.{nameof(User.UserName)}", chat.RecipientUser.UserName);
                    rmqMessage.PayLoad.Add($"{nameof(Chat.RecipientUser)}.{nameof(User.FullName)}", chat.RecipientUser.FullName);

                    if (session1 != null)
                        BLFactory.GetRmqConnector().PublishRouted(session1.ApiKey, rmqMessage);

                    if (session2 != null)
                        BLFactory.GetRmqConnector().PublishRouted(session2.ApiKey, rmqMessage);
                }
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

                // rmq publish if initiator user or/and recipient user online
                PublishChatStateChanged(chat);
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

                // rmq publish if initiator user or/and recipient user online
                PublishChatStateChanged(chat);
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

        private void PublishChatStateChanged(Chat chat)
        {
            IUserBL bl = BLFactory.CreateUserBL();
            UserSession session1 = bl.GetSessionByUser(chat.RecipientUserId);
            UserSession session2 = bl.GetSessionByUser(chat.InitiatorUserId);

            if (session1 != null || session2 != null)
            {
                RmqMessage rmqMessage = new RmqMessage(RmqMessageType.ChatStateChanged);
                rmqMessage.PayLoad.Add(nameof(Chat.ChatId), chat.ChatId.ToString());
                rmqMessage.PayLoad.Add(nameof(Chat.State), chat.State.ToString());
                rmqMessage.PayLoad.Add(nameof(Chat.InitiatorUserId), chat.InitiatorUserId.ToString());
                rmqMessage.PayLoad.Add(nameof(Chat.RecipientUserId), chat.RecipientUserId.ToString());

                if (session1 != null)
                    BLFactory.GetRmqConnector().PublishRouted(session1.ApiKey, rmqMessage);

                if (session2 != null)
                    BLFactory.GetRmqConnector().PublishRouted(session2.ApiKey, rmqMessage);
            }
        }
    }
}
