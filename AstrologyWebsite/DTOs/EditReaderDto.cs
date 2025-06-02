using AstrologyWebsite.Models;
using System.ComponentModel.DataAnnotations;

namespace AstrologyWebsite.DTOs
{
    public class EditReaderDto
    {
        public string? FullName { get; set; }

        public int? Gender { get; set; }
        public DateOnly? Dob { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }

        public int? Experience { get; set; }

        public AccountStatus? Status { get; set; }

        public int? IsDeleted { get; set; }
    }
}
