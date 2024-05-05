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

        [HttpGet("categories")]
        public async Task<JsonResult> GetCategoies(bool all)
        {
            return Json(await auctionService.GetCategories(all));
        }

        [HttpGet("conditions")]
        public async Task<JsonResult> GetConditions()
        {
            return Json(await auctionService.GetConditions());
        }

        [HttpGet("sorters")]
        public Task<JsonResult> GetSortWays()
        {
            return Task.FromResult(Json(auctionService.GetSortWays()));
        }
    }
}
