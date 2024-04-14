using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AuctionWebApp.Server.Model.AuctionTypes
{
    public class HiddenAuction : IAuctionActions
    {
        public Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, MySqlContext context, ulong? maxBid = null)
        {
            return new Task<Bid?>(() => null);
        }

        public async Task<bool> BidCheck(Lot lot, ulong amount, DateTime time, MySqlContext context)
        {
            return amount > await GetActualCost(lot, time, context);
        }

        public virtual async Task<Bid?> GetActualBid(Lot lot, MySqlContext context)
        {
            return await context.Bids
                                .Where(b => b.BLotId == lot.LId)
                                .OrderByDescending(b => b.BSize)
                                .ThenBy(b => b.BTime)
                                .FirstOrDefaultAsync();
        }

        public Task<ulong> GetActualCost(Lot lot, DateTime time, MySqlContext context)
        {
            return new Task<ulong>(() => lot.LInitialCost);
        }

        public (BigInteger, BigInteger) GetSimulationUserBounds(SimulationInfo simulationInfo, SimulationUser user)
        {
            var lowerBound = (BigInteger)user.EstimatedCost - simulationInfo.InitialPrice;
            var upperBound = (BigInteger)user.Budget - user.EstimatedCost;
            return (lowerBound, upperBound);
        }
    }
}
