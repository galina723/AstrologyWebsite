using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace AstrologyWebsite.Models
{
    public class AstroUser : IdentityUser
    {

        public string? FullName { get; set; }

        public int? Gender { get; set; }

        public DateOnly? Dob { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Image { get; set; }

        public AccountStatus Status { get; set; }

        public int? IsDeleted { get; set; }

        public int? RoleId { get; set; }

        public int? Experience { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

        [InverseProperty("Customer")]
        public virtual ICollection<Booking> BookingCustomers { get; set; } = new List<Booking>();

    }
}