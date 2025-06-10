using Newtonsoft.Json;

namespace AstrologyWebsite.Models
{
    public class HoroscopeResult
    {
        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("zodiac")]
        public string Zodiac { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("horoscope")]
        public string Horoscope { get; set; }
    }
}
