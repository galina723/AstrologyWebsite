using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using AstrologyWebsite.Models;
using Newtonsoft.Json;
namespace AstrologyWebsite.HoroscropServices
{
    public class ZodiacCompatibilityService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public ZodiacCompatibilityService(IConfiguration config)
        {
            var accessToken = config["DivineApi:AccessToken"];
            _apiKey = config["DivineApi:ApiKey"];
            var baseUrl = config["DivineApi:BaseUrl"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<ZodiacCompatibilityResult?> GetCompatibilityAsync(string sign1, string sign2)
        {
            var requestBody = new
            {
                api_key = _apiKey,
                sign_1 = sign1,
                sign_2 = sign2,
                lan = "en"
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(string.Empty, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Divine API error: {response.StatusCode} - {error}");
            }
            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RAW JSON: " + json);

            return ZodiacCompatibilityResult.FromApiJson(json);
        }


    }

}
