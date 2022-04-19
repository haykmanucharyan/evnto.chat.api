using evnto.chat.dal.Entities;

namespace evnto.chat.bll.Interfaces
{
    public interface IUserBL
    {
        void SignUp(string fullName, string userName, string password);

        string SignIn(string userName, string password);

        List<User> GetOnlineUsers(int userId);

        int GetAuthenticatedUser(string token);
    }
}
