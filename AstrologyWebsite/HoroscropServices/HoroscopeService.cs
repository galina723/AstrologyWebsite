using System.Net.Http;
using System.Threading.Tasks;
using AstrologyWebsite.Models;
using Newtonsoft.Json;

namespace AstrologyWebsite.HoroscropServices
{
    public class HoroscopeService
    {
        private readonly HttpClient _client;

        public HoroscopeService()
        {
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Add RapidAPI headers here
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "astropredict-daily-horoscopes-lucky-insights.p.rapidapi.com"); // Adjust this based on your API
            _client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "90c7a45f30mshacb44f7330f38e2p1808b8jsn6aa2bda46a24"); // Replace with your RapidAPI key
        }

        public async Task<HoroscopeResult> GetDailyHoroscopeAsync(string sign)
        {
            try
            {
                var date = DateTime.Today.ToString("yyyy-MM-dd");
                var url = $"https://astropredict-daily-horoscopes-lucky-insights.p.rapidapi.com/horoscope?lang=en&zodiac={sign}&type=daily&timezone=UTC?"; // Update with the correct endpoint

                var response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return new HoroscopeResult { Horoscope = $"Unable to fetch horoscope right now. Status: {response.StatusCode}" };

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<HoroscopeResult>(json);
                return result ?? new HoroscopeResult { Horoscope = "No horoscope found for this sign." };
            }
            catch (Exception ex)
            {
                // Log exception (optional)
                return new HoroscopeResult { Horoscope = "Error fetching horoscope. Please try again later." };
            }
        }
    }
}
