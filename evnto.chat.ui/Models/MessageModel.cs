namespace evnto.chat.ui.Models
{
    public class MessageModel
    {
        public long MessageId { get; set; }

        public int ChatId { get; set; }

        public int AuthorUserId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string Text { get; set; }

        public UserModel AuthorUser { get; set; }

        public string AuthorUserInfo => AuthorUser?.UserInfo;
    }
}
