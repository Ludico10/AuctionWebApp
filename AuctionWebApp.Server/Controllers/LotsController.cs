using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LotsController(IAuctionService auctionService) : Controller
    {
        [HttpPost("{lotId}")]
        public async Task<IActionResult> PostAsync(int lotId, BidRequest newBid)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceBid(newBid.LotId, newBid.UserId, newBid.Size, null);
                return Ok(newBid);
            }
            return BadRequest(ModelState);
        }
    }
}
