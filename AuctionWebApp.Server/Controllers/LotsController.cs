using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lots")]
    public class LotsController(IAuctionService auctionService) : Controller
    {
        private readonly int itemsOnPage = 10;

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BidRequest newBid)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceBid(newBid.LotId, newBid.UserId, newBid.Size, null);
                return Ok(newBid);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IEnumerable<LotShortInfo>> GetAsync(int pageNumber, ushort category)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid)
            {
                var lotsList = await auctionService.GetLotsPage(pageNumber, itemsOnPage, category);
                foreach ( var lot in lotsList )
                {
                    var cost = await auctionService.GetActualCost(lot);
                    result.Add(new LotShortInfo(lot, cost));
                }
            }

            return result;
        }

        [HttpGet("{lotId}")]
        public async Task<LotInfo?> GetAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                return await auctionService.GetLotInfo(lotId);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceLot(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await auctionService.ChangeLot(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid && lotInfo.Id != null)
            {
                await auctionService.RemoveLot(lotInfo.Id.Value, lotInfo.SellerId, lotInfo.InitialCost);
                //штраф
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }
    }
}
