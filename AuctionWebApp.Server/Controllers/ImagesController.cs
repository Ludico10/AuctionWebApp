using Microsoft.AspNetCore.Mvc;

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
    }
}
