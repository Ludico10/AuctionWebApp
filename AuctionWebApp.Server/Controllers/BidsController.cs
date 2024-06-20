using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("bids")]
    public class BidsController(IAuctionService auctionService) : Controller
    {
        [HttpPost()]
        public async Task<IActionResult> PlaceBidAsync([FromBody] BidRequest newBid)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceBid(newBid.LotId, newBid.UserId, newBid.Size, DateTime.Now, newBid.MaxSize);
                return Ok(newBid);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("{lotId}")]
        public async Task<IActionResult> GetCostAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                var cost = await auctionService.GetActualCost(lotId);
                if (cost != null) return Ok(cost);
                else return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("track/{lotId}")]
        public async Task<bool> GetTrackableAsync(ulong lotId, ulong userId)
        {
            return await auctionService.IsTrackable(lotId, userId);
        }

        [HttpPost("track/{lotId}")]
        public async Task<IActionResult> MakeTrackableAsync(ulong lotId, ulong userId, [FromBody] bool trackable)
        {
            if (ModelState.IsValid)
            {
                await auctionService.ChangeTrackable(lotId, userId, trackable);
                return Ok(trackable);
            }

            return BadRequest(ModelState);
        }
    }
}
