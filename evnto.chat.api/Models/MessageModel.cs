namespace evnto.chat.api.Models
{
    public class MessageModel
    {
        public long MessageId { get; set; }

        public int ChatId { get; set; }

        public int AuthorUserId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string Text { get; set; }
    }
}
