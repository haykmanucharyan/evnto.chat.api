namespace evnto.chat.ui.Models
{
    public class ChatModel
    {
        public int ChatId { get; set; }

        public int InitiatorUserId { get; set; }

        public int RecipientUserId { get; set; }

        public ChatState State { get; set; }

        public DateTimeOffset Created { get; set; }

        public UserModel InitiatorUser { get; set; }

        public UserModel RecipientUser { get; set; }

        public string ChatInfo { get; set; }
    }

    public enum ChatState : byte
    {
        Initiated = 0,
        Accepted = 1,
        Rejected = 2,
        Closed = 3
    }
}
