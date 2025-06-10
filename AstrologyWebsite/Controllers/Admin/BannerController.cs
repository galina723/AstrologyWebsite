using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Banner")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
        private readonly AstrologyDatabaseContext context;

        public BannerController(AstrologyDatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet("Banners")]
        public IActionResult Banners()
        {
            var banners = context.Banners.ToList();
            return View(banners);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.BannerTypes = GetBannerTypeSelectList();
            return View();
        }

        [HttpPost("Create")]
        public IActionResult Create(BannerDTO dto)
        {
            ViewBag.BannerTypes = GetBannerTypeSelectList();

            if (dto.ImageFile == null)
            {
                ModelState.AddModelError(nameof(dto.ImageFile), "Image is required.");
            }

            if (ModelState.IsValid)
            {
                var banner = new Banner
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    bannerType = dto.BannerType ?? BannerType.HeaderHome,
                    Image = SaveImageIfNotExists(dto.ImageFile)
                };
                context.Banners.Add(banner);
                context.SaveChanges();
                TempData["ToastMessage"] = "Banner created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Banners");
            }
            return View(dto);
        }

        [HttpGet("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var banner = context.Banners.Find(id);
            if (banner == null)
                return RedirectToAction("Banners");

            var dto = new BannerDTO
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                BannerType = banner.bannerType,
                ImageURL = banner.Image
            };
            ViewBag.BannerTypes = GetBannerTypeSelectList();
            return View(dto);
        }

        [HttpPost("Edit/{id}")]
        public IActionResult Edit(int id, BannerDTO dto)
        {
            ViewBag.BannerTypes = GetBannerTypeSelectList();

            if (ModelState.IsValid)
            {
                var banner = context.Banners.Find(id);
                if (banner == null)
                    return RedirectToAction("Banners");

                banner.Title = dto.Title;
                banner.Description = dto.Description;
                banner.bannerType = dto.BannerType ?? BannerType.HeaderHome;
                if (dto.ImageFile != null)
                {
                    banner.Image = SaveImageIfNotExists(dto.ImageFile);
                }
                context.SaveChanges();
                TempData["ToastMessage"] = "Banner updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Banners");
            }
            return View(dto);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var banner = context.Banners.Find(id);
            if (banner != null)
            {
                context.Banners.Remove(banner);
                context.SaveChanges();
                TempData["ToastMessage"] = "Banner deleted successfully!";
                TempData["ToastType"] = "success";
            }
            else
            {
                TempData["ToastMessage"] = "Banner not found.";
                TempData["ToastType"] = "error";
            }
            return RedirectToAction("Banners");
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

        private List<SelectListItem> GetBannerTypeSelectList()
        {
            return Enum.GetValues(typeof(BannerType))
                .Cast<BannerType>()
                .Select(bt => new SelectListItem
                {
                    Value = bt.ToString(),
                    Text = bt.ToString()
                }).ToList();
        }
    }
}
