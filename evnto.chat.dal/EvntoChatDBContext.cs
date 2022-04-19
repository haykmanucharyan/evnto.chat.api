using evnto.chat.dal.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.dal
{
    public class EvntoChatDBContext : DbContext
    {        
        private string _dbConnectionString;

        public DbSet<User> Users { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public EvntoChatDBContext(string connectionString)
        {
            _dbConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSession>().IsMemoryOptimized();
        }

        public void ExecSessionCreateSP(string token, int userId)
        {
            Database.ExecuteSqlRaw("EXEC dbo.SPCreateSession @Token, @UserId",
                new SqlParameter { ParameterName = "@Token", Value = token },
                new SqlParameter { ParameterName = "@UserId", Value = userId });
        }
    }
}
