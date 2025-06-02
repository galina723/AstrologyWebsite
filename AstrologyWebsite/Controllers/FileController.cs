using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers
{
        [Route("File")]
        public class FileController : Controller
        {
            [HttpPost("UploadImage")]
            public async Task<IActionResult> UploadImage(IFormFile file)
            {
                if (file != null && file.Length > 0)
                {
                    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var imageUrl = Url.Content("~/uploads/" + fileName);

                    return Json(new { link = imageUrl });
                }

                return BadRequest();
            }
        }
    
}
