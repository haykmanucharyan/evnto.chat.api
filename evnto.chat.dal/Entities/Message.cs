using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace evnto.chat.dal.Entities
{
    [Table("Message")]
    [Index(nameof(ChatId), Name = "IX_Message_ChatId")]
    public class Message
    {
        [Key]
        public long MessageId { get; set; }

        [Required]
        [ForeignKey("FK_Message_Chat")]
        public int ChatId { get; set; }

        [Required]
        [ForeignKey("FK_Message_User")]
        public int AuthorUserId { get; set; }

        [Column(TypeName = "DATETIMEOFFSET(3)")]
        [Required]
        public DateTimeOffset Created { get; set; }

        [Column(TypeName = "NVARCHAR(4000)")]
        [Required]
        public string Text { get; set; }

        public virtual User AuthorUser { get; set; }
    }
}
