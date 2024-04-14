using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("simulation")]
    public class SimulationController(IAuctionService auctionService, ISimulationService simulationService, MySqlContext context) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> PostAsync(SimulationInfo simulationInfo)
        {
            var dbUsers = await simulationService.FindTestUsers(context);
            if (dbUsers == null || dbUsers.Count < 3)
            {
                return BadRequest();
            }

            var lotInfo = simulationService.Preparation(simulationInfo, dbUsers[2]);
            var lot = await auctionService.PlaceLot(lotInfo);
            if (lot == null)
            {
                return BadRequest();
            }

            var bids = new List<SimulationBidInfo>();
            SimulationBidInfo? lastBid = null;
            for (int i = 1; i <= simulationInfo.CyclesCount; i++)
            {
                var bidInfo = simulationService.Process(simulationInfo, i);
                if (bidInfo != null 
                    && (lastBid == null || bidInfo.SimulationUserId != lastBid.SimulationUserId)
                    && await auctionService.PlaceBid(lot.LId, dbUsers[bids.Count % 2].UId, bidInfo.Size, DateTime.Now.AddHours(i)))
                {
                    lastBid = bidInfo;
                    bids.Add(bidInfo);
                }
            }

            await auctionService.CloseAuction(lot);
            return Json(bids);
        }
    }
}
