using Microsoft.AspNetCore.Mvc;
using AstrologyWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using AstrologyWebsite.DTOs;

namespace AstrologyWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AstroUser> _signInManager;
        private readonly UserManager<AstroUser> _userManager;
        public AccountController(SignInManager<AstroUser> signInManager, UserManager<AstroUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Invalid input: " + string.Join("; ", errors);
                return View("Login", model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                // Check if the account is suspended
                if (user.Status == AccountStatus.Suspended)
                {
                    TempData["ErrorMessage"] = "Your account has been suspended. Please contact support for assistance.";
                    return View("Login", model);
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            TempData["ErrorMessage"] = "Invalid login attempt!";
            return View("Login", model);
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var user = new AstroUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                FullName = model.Email,
                CreatedDate = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
                Console.WriteLine($"This is error | Error: {error.Description}");

            TempData["ErrorMessage"] = "Registration failed. Please try again!";
            return View("Model", model);
        }


    }
}
