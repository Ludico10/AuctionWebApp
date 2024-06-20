using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lists")]
    public class ListsController(ICatalogService catalogService, ICommunicationService communicationService) : Controller
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

        [HttpGet("info")]
        public async Task<JsonResult> GetInfoShortAsync()
        {
            return Json(await communicationService.GetNewsNames());
        }

        [HttpGet("info/{id}")]
        public async Task<JsonResult> GetSectionText(uint id)
        {
            return Json(await communicationService.GetNewText(id));
        }

        [HttpGet("complaints/reasons")]
        public async Task<JsonResult> GetComplaintReasonsAsync()
        {
            return Json(await catalogService.GetComplaintReasons());
        }

        [HttpPost("complaints")]
        public async Task<IActionResult> PlaceComplaintsAsync(ComplaintRequest complaintRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await catalogService.SendComplaint(complaintRequest);
                    return Ok();
                } 
                catch (Exception ex) 
                { 
                    BadRequest(ex);
                }
            }

            return BadRequest();
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

        [HttpPut("catalog/bids")]
        public async Task<IEnumerable<BidShortInfo>> GetBidShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<BidShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotBidsList = await catalogService.GetLotsWithBids(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                var lotInfo = new LotShortInfo((ulong)lotBidsList.Item2);
                result.Add(new BidShortInfo(lotInfo));
                foreach (var lbl in lotBidsList.Item1)
                {
                    lotInfo = new LotShortInfo(lbl.Item1.BLot, lbl.Item2);
                    result.Add(new BidShortInfo(lotInfo, lbl.Item1));
                }
            }

            return result;
        }

        [HttpPut("catalog/tracking")]
        public async Task<IEnumerable<TrackableShortInfo>> GetTrackableShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<TrackableShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotTrackableList = await catalogService.GetTrackableLots(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                var lotInfo = new LotShortInfo((ulong)(lotTrackableList.Item2));
                result.Add(new TrackableShortInfo(lotInfo, null));
                foreach (var ltl in lotTrackableList.Item1)
                {
                    lotInfo = new LotShortInfo(ltl.Item1.TlLot, ltl.Item2);
                    result.Add(new TrackableShortInfo(lotInfo, ltl.Item1.TlMaxAutomaticBid));
                }
            }

            return result;
        }

        [HttpPut("catalog/winned")]
        public async Task<IEnumerable<FinishedShortInfo>> GetWinnedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<FinishedShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var winnedList = await catalogService.GetUserFinishedLots(listInfo.UserId.Value, true, listInfo.ItemsOnPage, listInfo.PageNumber);
                result.Add(new FinishedShortInfo((ulong)winnedList.Item2));
                foreach (var winned in winnedList.Item1)
                {
                    result.Add(new FinishedShortInfo(winned));
                }
            }

            return result;
        }

        [HttpPut("catalog/owned")]
        public async Task<IEnumerable<LotShortInfo>> GetOwnedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var lotList = await catalogService.GetUserActualLots(listInfo.UserId.Value, listInfo.ItemsOnPage, listInfo.PageNumber);
                result.Add(new LotShortInfo((ulong)lotList.Item2));
                foreach (var lot in lotList.Item1)
                {
                    result.Add(new LotShortInfo(lot.Item1, lot.Item2));
                }
            }

            return result;
        }

        [HttpPut("catalog/closed")]
        public async Task<IEnumerable<FinishedShortInfo>> GetClosedShortsAsync(CatalogRequest listInfo)
        {
            var result = new List<FinishedShortInfo>();
            if (ModelState.IsValid && listInfo.UserId != null)
            {
                var closedList = await catalogService.GetUserFinishedLots(listInfo.UserId.Value, false, listInfo.ItemsOnPage, listInfo.PageNumber);
                result.Add(new FinishedShortInfo((ulong)closedList.Item2));
                foreach (var closed in closedList.Item1)
                {
                    result.Add(new FinishedShortInfo(closed));
                }
            }

            return result;
        }
    }
}
