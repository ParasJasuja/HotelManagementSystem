using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        public Menu()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [Column("MenuID")]
        public int MenuId { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        [Display(Name = "Type")]
        public string MenuType { get; set; } = null!;
        [Display(Name = "Description")]
        public string MenuDescription { get; set; } = null!;
        [Column(TypeName = "money")]
        [Display(Name = "Price")]
        public decimal MenuPrice { get; set; }
        [Display(Name = "Name")]
        public string MenuName { get; set; } = null!;

        [InverseProperty("Menu")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
