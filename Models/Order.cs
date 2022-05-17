using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models
{
    public partial class Order
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        [Column("MenuID")]
        public int MenuId { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string OrderStatus { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string PaymentStatus { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime OrderTime { get; set; }

        [ForeignKey("MenuId")]
        [InverseProperty("Orders")]
        public virtual Menu Menu { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("Orders")]
        public virtual User User { get; set; } = null!;
    }
}
