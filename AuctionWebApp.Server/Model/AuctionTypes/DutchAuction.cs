using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AuctionWebApp.Server.Model.AuctionTypes
{
    public class DutchAuction : IAuctionActions
    {
        public Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, MySqlContext context, ulong? maxBid = null)
        {
            Bid? bid = null;
            return Task.FromResult(bid);
        }

        public async Task<bool> BidCheck(Lot lot, ulong amount, DateTime time, MySqlContext context)
        {
            var bidCount = await context.Bids
                                        .Where(b => b.BLotId == lot.LId)
                                        .CountAsync();
            var actual = await GetActualCost(lot, time, context);
            return bidCount == 0 && amount >= actual;
        }

        public async Task<Bid?> GetActualBid(Lot lot, MySqlContext context)
        {
            return await context.Bids
                .Where(b => b.BLotId == lot.LId)
                .OrderByDescending(b => b.BSize)
                .ThenBy(b => b.BTime)
                .FirstOrDefaultAsync();
        }

        public Task<ulong> GetActualCost(Lot lot, DateTime time, MySqlContext context)
        {
            var hours = (lot.LFinishTime - lot.LStartTime).Hours;
            var leftHours = (lot.LFinishTime - time).Hours;
            decimal step = ((decimal)lot.LInitialCost - lot.LCostStep) / hours;
            return Task.FromResult(lot.LCostStep + Convert.ToUInt64(leftHours * step));
        }

        public (BigInteger, BigInteger) GetSimulationUserBounds(SimulationInfo simulationInfo, SimulationUser user)
        {
            var lastPrice = simulationInfo.InitialPrice - (BigInteger)simulationInfo.PriceStep * simulationInfo.CyclesCount;
            var lowerBound = (BigInteger)user.EstimatedCost - lastPrice;
            var upperBound = ((BigInteger)user.Budget - simulationInfo.InitialPrice > 0 ? (BigInteger)user.Budget :
                                                                                          (BigInteger)simulationInfo.InitialPrice);
            return (lowerBound, upperBound - user.EstimatedCost);
        }
    }
}
