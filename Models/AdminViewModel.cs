namespace HotelManagementSystem.Models
{
    public class AdminViewModel
    {
        public string Username { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        public string ReturnUrl { get; set; } = "/Admin/Dashboard";
    }
}
