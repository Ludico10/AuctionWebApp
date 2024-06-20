using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : Controller
    {
        [HttpGet("{type}")]
        public async Task GetImageAsync(string name, string type)
        {
            string path = Directory.GetCurrentDirectory() + "\\Images\\" + type + "\\" + name + ".png";
            //if (System.IO.File.Exists(path))
            {
                await HttpContext.Response.SendFileAsync(path);
            }
        }

        [HttpPost("upload")]
        public async Task PostImage()
        {
            var postedFile = HttpContext.Request.Form.Files["Image"];
            if (postedFile != null)
            {
                string path = Directory.GetCurrentDirectory() + "\\Images\\actual\\" + postedFile.FileName + ".png";
                using (Stream fileStream = new FileStream(path, FileMode.Create))
                {
                    await postedFile.CopyToAsync(fileStream);
                }
            }
        }
    }
}
