using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelManagementSystem.Utility;
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
        [Display(Name = "Order Status")]
        [ValidValue("Pending", "Approved", "Canceled", "Denied")]
        public string OrderStatus { get; set; } = "Pending";
        [StringLength(20)]
        [Unicode(false)]
        [Display(Name = "Payment Status")]
        [ValidValue("Pending", "Failed", "Canceled", "Success")]

        public string PaymentStatus { get; set; } = "Pending";
        [Column(TypeName = "datetime")]
        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }
        public int Quantity { get; set; } = 1;

        [ForeignKey("MenuId")]
        [InverseProperty("Orders")]
        public virtual Menu? Menu { get; set; } 
        [ForeignKey("UserId")]
        [InverseProperty("Orders")]
        public virtual User? User { get; set; }
    }
}
