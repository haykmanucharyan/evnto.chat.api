using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.dal.Entities
{
    [Table("UserSession")]
    [Index(nameof(UserId), Name = "IX_UserSession_UserId")]
    public class UserSession
    {
        [Key]
        public string Token { get; set; }
        
        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(256)")]
        public string ApiKey { get; set; }
    }
}
