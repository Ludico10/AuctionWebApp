using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lists")]
    public class ListsController(ICatalogService catalogService) : Controller
    {
        [HttpGet("auctionTypes")]
        public async Task<JsonResult> GetAuctionTypesAsync()
        {
            return Json(await catalogService.GetAuctionTypes());
        }

        [HttpGet("categories")]
        public async Task<JsonResult> GetCategoiesAsync(bool all)
        {
            return Json(await catalogService.GetCategories(all));
        }

        [HttpGet("conditions")]
        public async Task<JsonResult> GetConditionsAsync()
        {
            return Json(await catalogService.GetConditions());
        }

        [HttpGet("sorters")]
        public Task<JsonResult> GetSortWays()
        {
            return Task.FromResult(Json(catalogService.GetSortWays()));
        }

        [HttpPut("catalog")]
        public async Task<IEnumerable<LotShortInfo>> CatalogAsync([FromBody] CatalogRequest catalogInfo)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid)
            {
                var lotsList = await catalogService.GetLotsPage(catalogInfo);
                foreach (var lot in lotsList)
                {
                    result.Add(new LotShortInfo(lot.Item1, lot.Item2));
                }
            }

            return result;
        }
    }
}
