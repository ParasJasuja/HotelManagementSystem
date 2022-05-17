using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        [Key]
        public int RoomNo { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        [RegularExpression("AC|Non-AC", ErrorMessage = "Invalid Type")]
        public string RoomType { get; set; } = null!;
        public string RoomDescription { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal RoomCharges { get; set; }
        [Required]
        [Column("isOpen")]
        public bool? IsOpen { get; set; }

        [InverseProperty("RoomNoNavigation")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
