using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionActions
    {
        public bool BidCheck(Lot lot, ulong amount, DateTime time, Bid? lastBid);
        public Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, ulong? maxBid, MySqlContext context);
    }
}
