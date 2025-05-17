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
using AstrologyWebsite.DTOs;
using Microsoft.CodeAnalysis.Elfie.Model;
using System.Xml.Linq;
using System.Numerics;

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
        return View("~/Views/Admin/Bookings/Bookings.cshtml");
    }

    public IActionResult Blogs()
    {
        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }
    public IActionResult CreateBlog()
    {
        return View("~/Views/Admin/Blogs/CreateBlog.cshtml");
    }

    public IActionResult Planets()
    {
        return View("~/Views/Admin/Planets/Planets.cshtml");
    }

    public IActionResult Zodiacs()
    {
        return View("~/Views/Admin/Zodiacs/Zodiacs.cshtml");
    }

    public IActionResult CreateZodiac()
    {
        return View("~/Views/Admin/Zodiacs/CreateZodiac.cshtml");
    }
    public IActionResult CreatePlanet()
    {
        return View("~/Views/Admin/Planets/CreatePlanet.cshtml");
    }

    public IActionResult CreateReader()
    {
        return View("~/Views/Admin/Readers/CreateReader.cshtml");
    }

    public IActionResult AddReader(ReaderDTO readerDto)
    {
        if (ModelState.IsValid)
        {
            AstroUser newReader = new AstroUser()
            {
                FullName = readerDto.FullName,
                Gender = readerDto.Gender,
                PhoneNumber = readerDto.PhoneNumber.ToString(),
                Email = readerDto.Email,
                Dob = readerDto.Dob,
                IsDeleted= 0,
                Status = 1,
                RoleId= 1,
                Password = readerDto.Password,
            };

            context.AstroUsers.Add(newReader);

            context.SaveChanges();

            return RedirectToAction("Readers");
        }

        return View("~/Views/Admin/Readers/Readers.cshtml");
    }

    public IActionResult EditReader(string? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var reader = context.Users.Find(id);

        if (reader == null)
        {
            return RedirectToAction("Readers");
        }


        ReaderDTO newReader = new ReaderDTO()
        {
            Id = reader.Id,
            FullName = reader.FullName,
            Email = reader.Email,
            PhoneNumber = reader.PhoneNumber,
            Status = 1,
            Gender = reader.Gender,
            IsDeleted = 0,
            Dob = reader.Dob,
            Password = reader.Password,
        };



        return View("~/Views/Admin/Readers/EditReader.cshtml", newReader);
    }

    [HttpPost]
    public IActionResult EditReader(string id, ReaderDTO readerDto)
    {
        if (ModelState.IsValid)
        {

            var reader = context.Users.Find(id);

            reader.FullName = readerDto.FullName;
            reader.Gender = readerDto.Gender;
            reader.PhoneNumber = readerDto.PhoneNumber.ToString();
            reader.Email = readerDto.Email;
            reader.Dob = readerDto.Dob;
            reader.Password = readerDto.Password;
       

            context.SaveChanges();

            return RedirectToAction("Readers");
        }

        return View("~/Views/Admin/Readers/Readers.cshtml");
    }

    [HttpPost]
    public IActionResult DeleteReader(string id)
    {

        if (ModelState.IsValid)
        {
            var user = context.Users.Find(id);

            if (user != null)
            {
                context.Users.Remove(user);

                context.SaveChanges(true);

                return RedirectToAction("Readers");
            }
        }

        return View("~/Views/Admin/Readers/Readers.cshtml");
    }

    [Route("Admin/Readers")]
    public IActionResult GetReader()
    {
        var readers = context.AstroUsers.ToList();

        List<AstroUser> listReader = new List<AstroUser>();

        foreach (AstroUser reader in readers)
        {
            listReader.Add(new AstroUser
            {
                Id = reader.Id,
                FullName = reader.FullName,
                Email = reader.Email,
                PhoneNumber = reader.PhoneNumber,
                Status = 1,
                Gender = reader.Gender,
                IsDeleted = 0,
            });
        }

        return View("~/Views/Admin/Readers/Readers.cshtml", listReader);
    }

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
            AuthorId = blog.AuthorId,
        };



        return View("~/Views/Admin/Blogs/EditBlog.cshtml", newBlog);
    }

    [HttpPost]
    public IActionResult EditBlog(int id, BlogDTO newBlogItem)
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

    public IActionResult AddBlog(BlogDTO newBlogDTO)
    {
        if (ModelState.IsValid)
        {
            DateTime createTime = DateTime.Now;

            Blog newBlog = new Blog()
            {
                Title = newBlogDTO.Title,
                Content = newBlogDTO.Content,
                CreatedDate = createTime,
                AuthorId = 1
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

    [Route("Admin/Zodiacs")]
    public IActionResult GetZodiac()
    {
        var zodiacs = context.Zodiacs.ToList();

        List<ZodiacDTO> listZodiacs = new List<ZodiacDTO>();

        foreach (Zodiac zodiac in zodiacs)
        {
            listZodiacs.Add(new ZodiacDTO
            {
                Id = zodiac.Id,
                Name = zodiac.Name,
                Symbol = zodiac.Symbol,
                Modality = zodiac.Modality,
                StartDate = zodiac.StartDate,
                EndDate = zodiac.EndDate,
                Element = zodiac.Element,
            });
        }


        return View("~/Views/Admin/Zodiacs/Zodiacs.cshtml", listZodiacs);
    }

    public IActionResult AddZodiac(ZodiacDTO zodiac)
    {
        if (ModelState.IsValid)
        {
            Zodiac newZodiac = new Zodiac()
            {
                Name = zodiac.Name,
                Symbol = zodiac.Symbol,
                Modality = zodiac.Modality,
                StartDate = zodiac.StartDate,
                EndDate = zodiac.EndDate,
                Element = zodiac.Element,
            };

            context.Zodiacs.Add(newZodiac);

            context.SaveChanges();

            return RedirectToAction("Zodiacs");
        }

        return View("~/Views/Admin/Zodiacs/Zodiacs.cshtml");
    }


    public IActionResult EditZodiac(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var zodiac = context.Zodiacs.Find(id);

        if (zodiac == null)
        {
            return RedirectToAction("Zodiac");
        }


        ZodiacDTO newZodiac = new ZodiacDTO()
        {
          Name = zodiac.Name,
          Symbol = zodiac.Symbol,
          Modality = zodiac.Modality,
          StartDate = zodiac.StartDate,
          EndDate = zodiac.EndDate,
          Element = zodiac.Element,
        };

        return View("~/Views/Admin/Zodiacs/EditZodiac.cshtml", newZodiac);
    }

    [HttpPost]
    public IActionResult EditZodiac(int id, ZodiacDTO newZodiacItem)
    {
        if (ModelState.IsValid)
        {

            var zodiac = context.Zodiacs.Find(id);

            zodiac.Name = newZodiacItem.Name;
            zodiac.Symbol = newZodiacItem.Symbol;
            zodiac.Modality = newZodiacItem.Modality;
            zodiac.StartDate = newZodiacItem.StartDate;
            zodiac.EndDate = newZodiacItem.EndDate;
            zodiac.Element = newZodiacItem.Element;

            context.SaveChanges();

            return RedirectToAction("Zodiacs");
        }

        return View("~/Views/Admin/Zodiacs/EditZodiac.cshtml");
    }

    [HttpPost]
    public IActionResult DeleteZodiac(int id)
    {

        if (ModelState.IsValid)
        {
            var zodiac = context.Zodiacs.Find(id);

            if (zodiac != null)
            {
                context.Zodiacs.Remove(zodiac);

                context.SaveChanges(true);

                return RedirectToAction("Zodiacs");
            }
        }

        return View("~/Views/Admin/Zodiacs/Zodiacs.cshtml");
    }


    [Route("Admin/Planets")]
    public IActionResult GetPlanets()
    {
        var planets = context.Planets.ToList();

        List<PlanetDTO> listPlanets = new List<PlanetDTO>();

        foreach (Planet planet in planets)
        {
            listPlanets.Add(new PlanetDTO
            {
                Id = planet.Id,
                Name = planet.Name,
                Symbol = planet.Symbol,
                Description = planet.Description,
            });
        }


        return View("~/Views/Admin/Planets/Planets.cshtml", listPlanets);
    }

    public IActionResult AddPlanet(PlanetDTO planet)
    {
        if (ModelState.IsValid)
        {
            Planet newPlanet = new Planet()
            {
                Name = planet.Name,
                Symbol = planet.Symbol,
                Description = planet.Description,
              
            };

            context.Planets.Add(newPlanet);

            context.SaveChanges();

            return RedirectToAction("Planets");
        }

        return View("~/Views/Admin/Planets/Planet.cshtml");
    }

    public IActionResult EditPlanet(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var planet = context.Planets.Find(id);

        if (planet == null)
        {
            return RedirectToAction("Planet");
        }


        PlanetDTO newPlanet = new PlanetDTO()
        {
            Name = planet.Name,
            Symbol = planet.Symbol,
            Description = planet.Description,

        };

        return View("~/Views/Admin/Planets/EditPlanet.cshtml", newPlanet);
    }

    [HttpPost]
    public IActionResult EditPlanet(int id, PlanetDTO newPlanet)
    {
        if (ModelState.IsValid)
        {

            var planet = context.Planets.Find(id);

            planet.Name = newPlanet.Name;
            planet.Symbol = newPlanet.Symbol;
            planet.Description = newPlanet.Description;
           
            context.SaveChanges();

            return RedirectToAction("Planets");
        }

        return View("~/Views/Admin/Planets/EditPlanet.cshtml");
    }

    public IActionResult DeletePlanet(int id)
    {

        if (ModelState.IsValid)
        {
            var planet = context.Planets.Find(id);

            if (planet != null)
            {
                context.Planets.Remove(planet);

                context.SaveChanges(true);

                return RedirectToAction("Planets");
            }
        }

        return View("~/Views/Admin/Planets/Planets.cshtml");
    }



}
