using Microsoft.AspNetCore.Mvc;
using AstrologyWebsite.ViewModels;
using Microsoft.AspNetCore.Identity;
using AstrologyWebsite.Models;

namespace AstrologyWebsite.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<AstroUser> _signInManager;

        public AccountController(SignInManager<AstroUser> signInManager)
        {
            _signInManager = signInManager;   
        }
        public IActionResult LoginRegister()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountPageViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var result = await _signInManager.PasswordSignInAsync(model.Login.Email!, model.Login.Password!, model.Login.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Email or Password wrong");
                
            }
          
            return View("LoginRegister", model);
        }
        
    }
}
