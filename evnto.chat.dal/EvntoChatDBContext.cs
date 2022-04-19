using evnto.chat.dal.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.dal
{
    public class EvntoChatDBContext : DbContext
    {
        #region Fields

        private string _dbConnectionString;

        #endregion

        #region Properties

        public DbSet<User> Users { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        #endregion

        #region Ctor

        public EvntoChatDBContext(string connectionString)
        {
            _dbConnectionString = connectionString;
        }

        #endregion

        #region Event overrides

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSession>().IsMemoryOptimized();
        }

        #endregion

        #region Methods

        public void ExecSessionCreateSP(string token, int userId, string apiKey)
        {
            Database.ExecuteSqlRaw("EXEC dbo.SPCreateSession @Token, @UserId, @ApiKey",
                new SqlParameter { ParameterName = "@Token", Value = token },
                new SqlParameter { ParameterName = "@UserId", Value = userId },
                new SqlParameter { ParameterName = "@ApiKey", Value = apiKey });
        }

        #endregion
    }
}
