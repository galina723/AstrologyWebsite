using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers.Admin
{
    public class UsersController : Controller
    {
        private readonly SignInManager<AstroUser> _signInManager;
        private readonly UserManager<AstroUser> _userManager;
        private readonly AstrologyDatabaseContext _context;
        public UsersController(SignInManager<AstroUser> signInManager, UserManager<AstroUser> userManager, AstrologyDatabaseContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Users()
        {
            return View();
        }
       

        //[Route("Admin/Readers")]
        //public IActionResult GetReader()
        //{
           
        //}

    }
}
