using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Readers()
    {
        return View("~/Views/Admin/Readers/Readers.cshtml");
    }

    public IActionResult Users()
    {
        return View("~/Views/Admin/Users/Users.cshtml");
    }

    public IActionResult Booking()
    {
        return View("~/Views/Admin/Booking/Booking.cshtml");
    }

    public IActionResult Blogs()
    {
        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }
    public IActionResult CreateBlog()
    {
        return View("~/Views/Admin/Blogs/CreateBlog.cshtml");
    }
    public IActionResult EditBlog()
    {
        return View("~/Views/Admin/Blogs/EditBlog.cshtml");
    }
}