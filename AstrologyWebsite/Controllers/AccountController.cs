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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Logged in successfully!";
                    return RedirectToAction("Index", "Home");
                }


                TempData["ErrorMessage"] = "Invalid login attempt!";
            }

            return View("Login", model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
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
