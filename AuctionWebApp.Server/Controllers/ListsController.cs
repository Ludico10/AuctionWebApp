using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lists")]
    public class ListsController(IAuctionService auctionService) : Controller
    {
        [HttpGet("auctionTypes")]
        public async Task<JsonResult> GetAuctionTypes()
        {
            return Json(await auctionService.GetAuctionTypes());
        }
    }
}
