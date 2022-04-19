using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.dal.Entities
{
    [Table("User")]
    [Index(nameof(UserName), Name = "IX_User_UserName", IsUnique = true)]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "VARCHAR(50)")]
        [Required]
        public string UserName { get; set; }

        [Column(TypeName = "NVARCHAR(256)")]
        [Required]
        public string FullName { get; set; }

        [Column(TypeName = "CHAR(64)")]
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public int Salt { get; set; }

        [Required]
        public int SaltCount { get; set; }

        [Column(TypeName = "DATETIMEOFFSET(3)")]
        [Required]
        public DateTimeOffset Created { get; set; }
    }
}
