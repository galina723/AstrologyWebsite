using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AstrologyWebsite.DTOs
{
    public class ReaderDTO
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Passwords must be at least 5 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateOnly Dob { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public string? Image { get; set; }

        public int? Experience { get; set; }

        public AccountStatus Status { get; set; }

        public DateTime CreatedDate { get; set; }
        public int? IsDeleted { get; set; }

        public int? RoleId { get; set; }
    }

}
