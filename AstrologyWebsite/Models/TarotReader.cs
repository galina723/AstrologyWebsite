namespace AstrologyWebsite.Models
{
    public class TarotReader
    {
        public int Experience { get; set; }
        public string AstroUserId { get; set; }
        public AstroUser Reader { get; set; }
    }
}
