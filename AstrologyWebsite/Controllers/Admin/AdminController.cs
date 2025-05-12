using AstrologyWebsite.Migrations;
using System.Text.RegularExpressions;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AstrologyWebsite.Areas.Database.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class AdminController : Controller
{
    private readonly AstrologyDatabaseContext context;

    public AdminController (AstrologyDatabaseContext context)
    {
        this.context = context;
    }

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

    //public IActionResult EditBlog()
    //{


    //    return View("~/Views/Admin/Blogs/EditBlog.cshtml");
    //}

    public IActionResult EditBlog(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }


        List<Blog> blogs = new List<Blog>();

        var blog = context.Blogs.Find(id);

        if (blogs == null)
        {
            return RedirectToAction("Blogs");
        }


        Blog newBlog = new Blog()
        {
            Title = blog.Title,
            Content = blog.Content,
            CreatedDate = blog.CreatedDate,
            AuthorId = blog.AuthorId,
        };



        return View("~/Views/Admin/Blogs/EditBlog.cshtml", newBlog);
    }

    [HttpPost]
    public IActionResult EditBlog(int id, Blog newBlogItem)
    {
        if (ModelState.IsValid)
        {

            var blog = context.Blogs.Find(id);

            blog.Title = newBlogItem.Title;
            blog.Content = newBlogItem.Content;

            context.SaveChanges();

            return RedirectToAction("Blogs");
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }

    public IActionResult AddBlog(string title, string content)
    {
        if (ModelState.IsValid)
        {
            DateTime createTime = DateTime.Now;

            Blog newBlog = new Blog()
            {
                Title = title,
                Content = content,
                CreatedDate = createTime,
                AuthorId = 1,
                //AstroUserId = 1,
            };

            context.Blogs.Add(newBlog);

            context.SaveChanges();

            return RedirectToAction("Blogs");
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }

    [Route("Admin/Blogs")]
    public IActionResult GetBlog()
    {
        var blogs = context.Blogs.ToList();

        List<Blog> listBlog = new List<Blog>();

        foreach (Blog blog in blogs) {
            var contentT = Regex.Replace(blog.Content, "<.*?>", "");
            var content = contentT.Truncate(35);

            listBlog.Add(new Blog
            {
                Id = blog.Id,
                Title =blog.Title,
                Content = content,
                CreatedDate = blog.CreatedDate,
                AuthorId = blog.AuthorId,
            });
        }


        return View("~/Views/Admin/Blogs/Blogs.cshtml", listBlog);
    }

    [HttpPost]
    public IActionResult DeleteBlog(int id)
    {

        if (ModelState.IsValid)
        {
            var blog = context.Blogs.Find(id);

            if(blog != null)
            {
                context.Blogs.Remove(blog);

                context.SaveChanges(true);

                return RedirectToAction("Blogs");
            }
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }
}
