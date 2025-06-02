using AstrologyWebsite.Models;

namespace AstrologyWebsite.DTOs
{
    public class ZodiacDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Symbol { get; set; }

        public string? Modality { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string? Element { get; set; }
        public IFormFile? Avatar {  get; set; }
        public string? AvatarUrl { get; set; }
    }

}


