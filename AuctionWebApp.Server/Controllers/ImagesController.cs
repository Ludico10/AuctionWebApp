using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("images")]
    public class ImagesController : Controller
    {
        [HttpGet("{type}")]
        public async Task GetImageAsync(string type, ulong sourceId)
        {
            string path = Directory.GetCurrentDirectory() + "\\Images\\" + type + "\\" + sourceId.ToString() + ".png";
            //if (System.IO.File.Exists(path))
            {
                await HttpContext.Response.SendFileAsync(path);
            }
        }
    }
}
