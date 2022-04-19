﻿using System.ComponentModel.DataAnnotations;
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

        public int UserId { get; set; }
    }
}