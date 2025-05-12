using AstrologyWebsite.Migrations;
using System.Text.RegularExpressions;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

public class AdminController : Controller
{
    SqlConnection con = new SqlConnection("Data Source=DESKTOP-C9H42P0;Initial Catalog=AstrologyDatabase;Integrated Security=True;Trust Server Certificate=True");

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
    public IActionResult EditBlog(int? id)
    {
        if (id == null)
        { 
            return BadRequest(); 
        }


        List<Blog> blogs = new List<Blog>();

        String sqlQuery = "SELECT * FROM Blogs";

        con.Open();

        SqlCommand sc = new SqlCommand(sqlQuery, con);

        SqlDataReader rd = sc.ExecuteReader();

        if (rd.Read())
        {
            while (rd.Read())
            {
                if(Convert.ToInt32(rd["Id"]) == id)
                {
                    DateTime date = Convert.ToDateTime(rd["CreatedDate"]);

                    blogs.Add(new Blog
                    {
                        Id = Convert.ToInt32(rd["Id"]),
                        Title = rd["Title"].ToString(),
                        Content = rd["Content"].ToString(),
                        CreatedDate = date,
                        AuthorId = Convert.ToInt32(rd["AuthorId"]),
                    });
                }
                
            }
        }

        con.Close();

        if (blogs.IsNullOrEmpty())
        {
            return NotFound();
        }

        return View("~/Views/Admin/Blogs/EditBlog.cshtml", blogs[0]);
    }

    [HttpPost]
    public IActionResult EditBlog(Blog blog)
    {
        if (ModelState.IsValid)
        { 

            DateTime createTime = DateTime.Now;
            String sqlQuery = "UPDATE Blogs SET title = '" + blog.Title + "', content = '" + blog.Content + "' WHERE Id = " + blog.Id;

            con.Open();

            SqlCommand sc = new SqlCommand(sqlQuery, con);

            sc.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Blogs");
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }

    public IActionResult AddReader(string fullName, int gender, string phone, string email, DateTime dob)
    {
        // String SqlQuery = "INSERT INTO User"

        return View("~/Views/Admin/Readers/Readers.cshtml");
    }
    public IActionResult AddBlog(String title, String content)
    {
        if (ModelState.IsValid)
        {
            DateTime createTime = DateTime.Now;
            String sqlQuery = "INSERT INTO Blogs (title, content, createdDate, authorId) VALUES ('" + title + "', '" + content + "', '" + createTime + "', '" + 1 + "')";

            con.Open();

            SqlCommand sc = new SqlCommand(sqlQuery, con);

            sc.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Blogs");
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }

    [Route("Admin/Blogs")]
    public IActionResult GetBlog()
    {
        List<Blog> blogs = new List<Blog>();

        String sqlQuery = "SELECT * FROM Blogs";

        con.Open();

        SqlCommand sc = new SqlCommand(sqlQuery, con);

        SqlDataReader rd = sc.ExecuteReader();

        if (rd.Read())
        {
            while (rd.Read())
            {
                DateTime date = Convert.ToDateTime(rd["CreatedDate"]);
                String contentT = Regex.Replace(rd["Content"].ToString(), "<.*?>", String.Empty);
                String content = contentT.Truncate(35);

                blogs.Add(new Blog
                {
                    Id = Convert.ToInt32(rd["Id"]),
                    Title = rd["Title"].ToString(),
                    Content = content,
                    CreatedDate = date,
                    AuthorId = Convert.ToInt32(rd["AuthorId"]),
                });
            }
        }

        con.Close();

        return View("~/Views/Admin/Blogs/Blogs.cshtml", blogs);
    }

    [HttpPost]
    public IActionResult DeleteBlog(String id)
    {

        if (ModelState.IsValid)
        {
            String sqlQuery = "DELETE FROM Blogs WHERE ID = '" + id + "'";

            con.Open();

            SqlCommand sc = new SqlCommand(sqlQuery, con);

            sc.ExecuteNonQuery();

            con.Close();

            return RedirectToAction("Blogs");
        }

        return View("~/Views/Admin/Blogs/Blogs.cshtml");
    }
}
