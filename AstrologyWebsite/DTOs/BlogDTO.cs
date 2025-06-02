using AstrologyWebsite.Models;

namespace AstrologyWebsite.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? AuthorId { get; set; }
        public IFormFile? Avatar {  get; set; }
        public string? AvatarURL {  get; set; }

        // public int? AstroUserId { get; set; }

        //public virtual User? Author { get; set; }
    }
}
