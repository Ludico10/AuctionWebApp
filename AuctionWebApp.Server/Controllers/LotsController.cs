using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("/bid")]
    public class LotsController(IAuctionService auctionService) : Controller
    {
        [HttpPost("{lotId}")]
        public async Task<IActionResult> PostAsync(ulong lotId , Bid newBid)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceBid(lotId, newBid.BParticipantId, newBid.BSize, null);
                return Ok(newBid);
            }
            return BadRequest(ModelState);
        }
    }
}
