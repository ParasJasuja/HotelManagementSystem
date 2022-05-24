using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Utility;

namespace HotelManagementSystem.Models
{
    public partial class Booking
    {
        [Key]
        [Column("BookingID")]
        public int BookingId { get; set; }
        public int RoomNo { get; set; }
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        [ValidValue("Pending", "Approved", "Canceled", "Denied")]
        public string BookingStatus { get; set; } = "Pending";
        [StringLength(100)]
        [Unicode(false)]
        [ValidValue("Pending", "Failed", "Canceled", "Success")]
        public string PaymentStatus { get; set; } = "Pending";
        [Column(TypeName = "datetime")]
        public DateTime DurationStart { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DurationEnd { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BookingTime { get; set; }

        [ForeignKey("RoomNo")]
        [InverseProperty("Bookings")]
        public virtual Room? RoomNoNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Bookings")]
        public virtual User? User { get; set; }
    }
}
