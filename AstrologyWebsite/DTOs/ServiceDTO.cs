 using System.ComponentModel.DataAnnotations;
using AstrologyWebsite.Models;

namespace AstrologyWebsite.DTOs
{ 
        public class ServiceDTO
        {
            public int Id { get; set; }

            [Required]
            public string ServiceName { get; set; }

            public string? Description { get; set; }

            public ServiceType? Type { get; set; }

            public int? Duration { get; set; }

            public int? Price { get; set; }

            public string? AvatarURL { get; set; }

            public IFormFile? Avatar { get; set; }
        
        }

}
