namespace evnto.chat.ui.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string UserInfo => $"{FullName} ({UserName})";
    }
}
