using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Model.AuctionTypes
{
    public class HiddenSecondPriceAuction : HiddenAuction
    {
        public override async Task<Bid?> GetActualBid(Lot lot, MySqlContext context)
        {
            var winnerBid = await context.Bids
                                .Where(b => b.BLotId == lot.LId)
                                .OrderByDescending(b => b.BSize)
                                .ThenBy(b => b.BTime)
                                .FirstOrDefaultAsync();
            if (winnerBid == null)
            {
                return null;
            }

            var priceBid = await context.Bids
                                .Where(b => b.BLotId == lot.LId && b.BSize < winnerBid.BSize)
                                .OrderByDescending(b => b.BSize)
                                .ThenBy(b => b.BTime)
                                .FirstOrDefaultAsync();
            var price = (priceBid == null) ? await GetActualCost(lot, DateTime.Now, context) : priceBid.BSize;

            return new Bid()
            {
                BParticipantId = winnerBid.BParticipantId,
                BSize = price
            };
        }
    }
}
