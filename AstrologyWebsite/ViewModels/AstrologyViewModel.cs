using AstrologyWebsite.Models;

namespace AstrologyWebsite.ViewModels
{
    public class AstrologyViewModel
    {
        public List<Zodiac> AllZodiac {  get; set; }
        public List<Zodiac> FireZodiac {  get; set; }
        public List<Zodiac> WaterZodiac {  get; set; }
        public List<Zodiac> AirZodiac {  get; set; }
        public List<Zodiac> EarthZodiac {  get; set; }
        public List<Planet> AllPlanets {  get; set; }
        
    }
}
