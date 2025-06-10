using AstrologyWebsite.HoroscropServices;
using Microsoft.AspNetCore.Mvc;
using AstrologyWebsite.Models;

namespace AstrologyWebsite.Controllers
{
    public class CompatibilityController : Controller
    {
        private readonly ZodiacCompatibilityService _compatibilityService;

        public CompatibilityController(ZodiacCompatibilityService compatibilityService)
        {
            _compatibilityService = compatibilityService;
        }

        [HttpGet]
        public async Task<IActionResult> Check(string sign1, string sign2)
        {
            try
            {
                var result = await _compatibilityService.GetCompatibilityAsync(sign1, sign2);
                if (result == null)
                    return Content("<span class='text-danger'>No compatibility result returned.</span>");

                return PartialView(result); 
            }
            catch (Exception ex)
            {
                return Content($"<span class='text-danger'>Error: {ex.Message}</span>");
            }
        }
    }

}
