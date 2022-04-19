using evnto.chat.bll.Interfaces;
using evnto.chat.dal;
using evnto.chat.dal.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.bll.Implementations
{
    internal class UserBL : BaseBL, IUserBL
    {
        internal UserBL(BLConfiguration config) : base(config)
        {
        }

        public string SignIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(Errors.EmptyUserName);

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(Errors.EmptyPassword);

            using (EvntoChatDBContext context = CreateDbContext())
            {
                User dbUser = context.Users.FirstOrDefault(u => u.UserName == userName);

                if (dbUser == null)
                    throw new Exception(Errors.UserNotFound);

                string hash = SecurityHelper.ComputePassowrdHash(password, dbUser.Salt, dbUser.SaltCount);
                if (!hash.Equals(dbUser.PasswordHash))
                    throw new Exception(Errors.IncorrectPassword);

                string token = SecurityHelper.ComputePassowrdHash($"{dbUser.UserId}_{DateTimeOffset.Now.Ticks}", dbUser.Salt, dbUser.SaltCount);

                // UserSession is in memory table, it's isolation level is snapshot,
                // so to delete an old session and create a new one atomically native compiled stored procedure is used,
                // because can't use EF's transaction
                context.ExecSessionCreateSP(token, dbUser.UserId);

                return token;
            }
        }

        public void SignUp(string fullName, string userName, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(fullName))
                    throw new ArgumentNullException(Errors.EmptyFullName);

                if (string.IsNullOrEmpty(userName))
                    throw new ArgumentNullException(Errors.EmptyUserName);

                if (string.IsNullOrEmpty(password))
                    throw new ArgumentNullException(Errors.EmptyPassword);

                User user = new User();
                user.FullName = fullName;
                user.UserName = userName;
                user.Created = DateTimeOffset.Now;
                user.Salt = SecurityHelper.GenerateSalt();
                user.SaltCount = SecurityHelper.GenerateSaltCount();
                user.PasswordHash = SecurityHelper.ComputePassowrdHash(password, user.Salt, user.SaltCount);

                using (EvntoChatDBContext context = CreateDbContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(SqlException))
                {
                    int num = (ex.InnerException as SqlException).Number;

                    if(num == 2627 || num == 2601)
                        throw new Exception(Errors.DuplicateUsername);
                }

                throw;
            }
            catch
            {
                throw;
            }
        }

        public List<User> GetOnlineUsers(int userId)
        {
            using (EvntoChatDBContext context = CreateDbContext())
            {
                return (from us in context.UserSessions
                        join u in context.Users on us.UserId equals u.UserId
                        where u.UserId != userId
                        select u).ToList();
            }
        }

        public int GetAuthenticatedUser(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(Errors.InvalidToken);

            using (EvntoChatDBContext context = CreateDbContext())
            {
                UserSession us = context.UserSessions.FirstOrDefault(us => us.Token == token);

                if (us != null)
                    return us.UserId;

                return 0;
            }
        }
    }
}
