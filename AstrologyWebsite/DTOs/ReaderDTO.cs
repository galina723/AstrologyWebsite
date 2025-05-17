using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace AstrologyWebsite.DTOs
{
    public class ReaderDTO: IdentityUser
        {
            public string Id { get; set; }

            public string? FullName { get; set; }

            public string? Password { get; set; }

            public int? Gender { get; set; }

            public DateOnly? Dob { get; set; }

            public string? PhoneNumber { get; set; }

            public string? Image { get; set; }

            public string? Email { get; set; }

            public byte? Status { get; set; }

            public int? IsDeleted { get; set; }

            public int? RoleId { get; set; }
    }
}
