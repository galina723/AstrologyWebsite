namespace AstrologyWebsite.DTOs
{
    public class PlanetDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Symbol { get; set; }

        public string? Description { get; set; }

        public IFormFile? Avatar {  get; set; }

        public string? AvatarURL { get; set; }
    }
}
