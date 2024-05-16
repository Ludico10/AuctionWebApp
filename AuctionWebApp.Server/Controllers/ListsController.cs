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

        [HttpGet("categories/premium")]
        public async Task<IEnumerable<PremiumInfo>> GetPremiumInfosAsync()
        {
            return await catalogService.GetPremiumCategories();
        }

        [HttpGet("conditions")]
        public async Task<JsonResult> GetConditionsAsync()
        {
            return Json(await catalogService.GetConditions());
        }

        [HttpGet("deliveries")]
        public async Task<JsonResult> GetDeliveriesAsync()
        {
            return Json(await catalogService.GetDeliveries());
        }

        [HttpGet("sorters")]
        public Task<JsonResult> GetSortWays()
        {
            return Task.FromResult(Json(catalogService.GetSortWays()));
        }

        [HttpPut("catalog")]
        public async Task<IEnumerable<LotShortInfo>> CatalogAsync(CatalogRequest catalogInfo)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid)
            {
                var lotsList = await catalogService.GetLotsPage(catalogInfo);
                result.Add(new LotShortInfo((ulong)lotsList.Item2));
                foreach (var lot in lotsList.Item1)
                {
                    result.Add(new LotShortInfo(lot.Item1, lot.Item2));
                }
            }

            return result;
        }

        [HttpGet("catalog/bids")]
        public async Task<IEnumerable<BidShortInfo>> GetBidShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<BidShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotBidsList = await catalogService.GetLotsWithBids(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                foreach (var lbl in lotBidsList)
                {
                    var lotInfo = new LotShortInfo(lbl.Item1.BLot, lbl.Item2);
                    result.Add(new BidShortInfo(lotInfo, lbl.Item1));
                }
            }

            return result;
        }

        [HttpGet("catalog/tracking")]
        public async Task<IEnumerable<TrackableShortInfo>> GetTrackableShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<TrackableShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotTrackableList = await catalogService.GetTrackableLots(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                foreach (var ltl in lotTrackableList)
                {
                    var lotInfo = new LotShortInfo(ltl.Item1.TlLot, ltl.Item2);
                    result.Add(new TrackableShortInfo(lotInfo, ltl.Item1.TlMaxAutomaticBid));
                }
            }

            return result;
        }

        [HttpGet("ctalog/winned")]
        public async Task<IEnumerable<FinishedShortInfo>> GetWinnedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<FinishedShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var winnedList = await catalogService.GetUserFinishedLots(listInfo.UserId.Value, true, listInfo.ItemsOnPage, listInfo.PageNumber);
                foreach (var winned in winnedList)
                {
                    result.Add(new FinishedShortInfo(winned));
                }
            }

            return result;
        }

        [HttpGet("catalog/owned")]
        public async Task<IEnumerable<LotShortInfo>> GetOwnedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotList = await catalogService.GetUserActualLots(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                foreach (var lot in lotList)
                {
                    result.Add(new LotShortInfo(lot.Item1, lot.Item2));
                }
            }

            return result;
        }

        [HttpGet("catalog/closed")]
        public async Task<IEnumerable<FinishedShortInfo>> GetClosedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<FinishedShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var closedList = await catalogService.GetUserFinishedLots(listInfo.UserId.Value, false, listInfo.ItemsOnPage, listInfo.PageNumber);
                foreach (var closed in closedList)
                {
                    result.Add(new FinishedShortInfo(closed));
                }
            }

            return result;
        }
    }
}
