using System.ComponentModel.DataAnnotations;
using AstrologyWebsite.Models;

namespace AstrologyWebsite.ViewModels
{
    public class MyAccountViewModel
    {
        [Required]
        public string? FullName { get; set; }

        public int? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? Dob { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Image { get; set; }

        // Password change fields
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public IEnumerable<Booking>? Bookings { get; set; }

    }
}
