using AstrologyWebsite.Migrations;
using System.Text.RegularExpressions;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.CodeAnalysis.Elfie.Model;
using System.Xml.Linq;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Blog")]
    [Authorize(Roles ="Admin")]
    public class BlogController : Controller
    {
        private readonly AstrologyDatabaseContext context;
        private readonly UserManager<AstroUser> _userManager;

        public BlogController(AstrologyDatabaseContext context, UserManager<AstroUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        [HttpGet("Blogs")]
        public async Task<IActionResult> Blogs()
        {
            var blogs = await context.Blogs
                .Include(b => b.Author)
                .ToListAsync();

            List<Blog> listBlog = new List<Blog>();

            foreach (Blog blog in blogs)
            {
                var contentT = Regex.Replace(blog.Content, "<.*?>", "");
                var content = contentT.Truncate(35);

                listBlog.Add(new Blog
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Content = content,
                    CreatedDate = blog.CreatedDate,
                    AuthorId = blog.AuthorId,
                    Author = blog.Author,
                    Avatar = blog.Avatar
                });
            }

            return View(listBlog);
        }

        [HttpGet("CreateBlog")]
        public IActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost("EditBlog/{id}")]
        public IActionResult EditBlog(int id, BlogDTO newBlogItem)
        {
            if (ModelState.IsValid)
            {

                var blog = context.Blogs.Find(id);

                blog.Title = newBlogItem.Title;
                blog.Content = newBlogItem.Content;

                if (newBlogItem.Avatar != null)
                {
                    blog.Avatar = SaveImageIfNotExists(newBlogItem.Avatar);
                }

                context.SaveChanges();

                return RedirectToAction("Blogs");
            }

            return View();
        }
        [HttpGet("EditBlog/{id}")]

        public IActionResult EditBlog(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var blog = context.Blogs.Find(id);

            if (blog == null)
            {
                return RedirectToAction("Blogs");
            }


            BlogDTO newBlog = new BlogDTO()
            {
                Title = blog.Title,
                Content = blog.Content,
                CreatedDate = blog.CreatedDate,
                AvatarURL = blog.Avatar
            };
            TempData["ToastMessage"] = "Blog updated successfully!";
            TempData["ToastType"] = "success";
            return View(newBlog);
        }
        [HttpPost("CreateBlog")]
        public async Task<IActionResult> AddBlog(BlogDTO newBlogDTO)
        {
            var user = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                DateTime createTime = DateTime.Now;

                Blog newBlog = new Blog()
                {
                    Title = newBlogDTO.Title,
                    Content = newBlogDTO.Content,
                    CreatedDate = createTime,
                    AuthorId = user.Id,
                };
                
                string avatar = SaveImageIfNotExists(newBlogDTO.Avatar);
                newBlog.Avatar = avatar;

                context.Blogs.Add(newBlog);

                context.SaveChanges();
                TempData["ToastMessage"] = "Blog created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Blogs");
            }

            return View();
        }

       

        [HttpPost]
        public IActionResult DeleteBlog(int id)
        {


            if (ModelState.IsValid)
            {
                var blog = context.Blogs.Find(id);
                if (blog != null)
                {
                    context.Blogs.Remove(blog);
                    context.SaveChanges(true);

                    TempData["ToastMessage"] = "Blog deleted successfully!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Blogs");
                }
            }
            TempData["ToastMessage"] = "Blog not found.";
            TempData["ToastType"] = "error";
            return View();
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
