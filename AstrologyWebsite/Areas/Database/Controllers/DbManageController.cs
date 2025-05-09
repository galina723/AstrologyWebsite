using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AstrologyDatabaseContext _dbContext;

        public DbManageController(AstrologyDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
           var success = await _dbContext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Delete Successfully" : "Cannot Delete";
            return RedirectToAction(nameof(Index)); 
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Update Successfully";
            return RedirectToAction(nameof(Index));
        }
    }

}
