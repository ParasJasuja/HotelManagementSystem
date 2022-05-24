using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models
{
    [Table("AdminUser")]
    public partial class AdminUser
    {
        [Key]
        [Column("AdminID")]
        public int AdminId { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Username { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
    }
}
