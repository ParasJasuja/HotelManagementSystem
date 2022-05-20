using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models
{
    [Index("PhoneNumber", Name = "U_PhoneNumber", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("UserID")]
        public int UserId { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; } = null!;
        [StringLength(30)]
        [Unicode(false)]
        public string Gender { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool PhoneNoConfirmed { get; set; }
        [Unicode(false)]
        [StringLength(12,ErrorMessage ="Length should be between 10 and 12",MinimumLength =10)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Password")]
        [MinLength(10, ErrorMessage ="Minimum Length Should be 8 Character")]
        public string HashPassword { get; set; } = null!;

        [InverseProperty("User")]
        public virtual ICollection<Booking> Bookings { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
