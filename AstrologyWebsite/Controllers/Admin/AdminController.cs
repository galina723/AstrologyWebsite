using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}