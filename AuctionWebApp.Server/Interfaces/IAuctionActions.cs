using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using System.Numerics;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionActions
    {
        public Task<bool> BidCheck(Lot lot, ulong amount, DateTime time, MySqlContext context);
        public Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, MySqlContext context, ulong? maxBid = null);
        public Task<ulong> GetActualCost(Lot lot, DateTime time, MySqlContext context);
        public Task<Bid?> GetActualBid(Lot lot, MySqlContext context);
        public (BigInteger, BigInteger) GetSimulationUserBounds(SimulationInfo simulationInfo, SimulationUser user);
    }
}
