using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace evnto.chat.dal.Entities
{
    [Table("Chat")]
    [Index(nameof(InitiatorUserId), Name = "IX_Chat_InitiatorUserId")]
    [Index(nameof(RecipientUserId), Name = "IX_Chat_RecipientUserId")]
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [ForeignKey("FK_Chat_User_Initiator")]
        [Required]
        public int InitiatorUserId { get; set; }

        [ForeignKey("FK_Chat_User_Recipient")]
        [Required]
        public int RecipientUserId { get; set; }

        [Required]
        public byte State { get; set; }

        [Column(TypeName = "DATETIMEOFFSET(3)")]
        [Required]
        public DateTimeOffset Created { get; set; }
    }
}
