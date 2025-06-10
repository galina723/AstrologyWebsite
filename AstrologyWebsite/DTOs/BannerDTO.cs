using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Http;

namespace AstrologyWebsite.DTOs
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public BannerType? BannerType { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageURL { get; set; }
    }
}
