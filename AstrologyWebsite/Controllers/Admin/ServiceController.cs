using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Service")]
    public class ServiceController : Controller
    {
        private readonly AstrologyDatabaseContext context;

        public ServiceController(AstrologyDatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet("Services")]
        public async Task<IActionResult> Services()
        {
            var services = await context.Services.ToListAsync();
            return View(services);
        }

        [HttpGet("CreateService")]
        public IActionResult CreateService()
        {
            ViewBag.ServiceTypes = Enum.GetValues(typeof(ServiceType))
                            .Cast<ServiceType>()
                    .Select(e => new SelectListItem
                     {
                       Value = e.ToString(),
                       Text = e.ToString()
                    }).ToList();
            return View();
        }

        [HttpPost("CreateService")]
        public async Task<IActionResult> AddService(ServiceDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateService");
            }

            var service = new Service
            {
                ServiceName = serviceDto.ServiceName,
                Description = serviceDto.Description,
                Type = serviceDto.Type,
                Duration = serviceDto.Duration,
                Price = serviceDto.Price,
                Avatar = SaveImageIfNotExists(serviceDto.Avatar)
            };

            context.Services.Add(service);
            await context.SaveChangesAsync();
            TempData["ToastMessage"] = "Service created successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Services");
        }

        [HttpGet("EditService/{id}")]
        public IActionResult EditService(int? id)
        {
            if (id == null)
                return BadRequest();

            var service = context.Services.Find(id);
            if (service == null)
                return RedirectToAction("Services");

            var dto = new ServiceDTO
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                Description = service.Description,
                Type = service.Type,
                Duration = service.Duration,
                Price = service.Price,
                AvatarURL = service.Avatar
            };
            ViewBag.ServiceTypes = Enum.GetValues(typeof(ServiceType))
                     .Cast<ServiceType>()
                     .Select(e => new SelectListItem
                       {
                           Value = e.ToString(),
                           Text = e.ToString()
                       }).ToList();

            return View(dto);
        }

        [HttpPost("EditService/{id}")]
        public async Task<IActionResult> EditService(int id, ServiceDTO serviceDto)
        {
            if (!ModelState.IsValid)
                return View(serviceDto);

            var service = context.Services.Find(id);
            if (service == null)
                return RedirectToAction("Services");

            service.ServiceName = serviceDto.ServiceName;
            service.Description = serviceDto.Description;
            service.Type = serviceDto.Type;
            service.Duration = serviceDto.Duration;
            service.Price = serviceDto.Price;
            if (serviceDto.Avatar != null)
                service.Avatar = SaveImageIfNotExists(serviceDto.Avatar);

            await context.SaveChangesAsync();
            TempData["ToastMessage"] = "Service updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Services");
        }

        [HttpPost("DeleteService")]
        public IActionResult DeleteService(int id)
        {
            var service = context.Services.Find(id);
            if (service != null)
            {
                context.Services.Remove(service);
                context.SaveChanges();

                TempData["ToastMessage"] = "Service deleted successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Services");
            }
            TempData["ToastMessage"] = "Service not found.";
            TempData["ToastType"] = "error";
            return RedirectToAction("Services");
        }
        private string SaveImageIfNotExists(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image");
            Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);
            }

            return "/image/" + imageFile.FileName;
        }
    }
}
