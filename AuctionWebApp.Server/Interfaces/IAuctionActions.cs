using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionActions
    {
        public Task<bool> BidCheck(Lot lot, ulong amount, DateTime time, MySqlContext context);
        public Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, ulong? maxBid, MySqlContext context);
        public Task<ulong> GetActualCost(Lot lot, MySqlContext context);
    }
}
