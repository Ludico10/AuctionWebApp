using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("simulation")]
    public class SimulationController(IAuctionService auctionService, ILotService lotService, ISimulationService simulationService, MySqlContext context) : Controller
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
            var lot = await lotService.Place(lotInfo);
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

            var result = new SimulationResult
            {
                Bids = bids,
                ResultCost = await auctionService.CloseAuction(lot)
            };
            simulationService.DetermineWinner(result);
            return Json(result);
        }
    }
}
