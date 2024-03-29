using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class AuctionService(MySqlContext context) : IAuctionService
    {
        public async Task PlaceLot()
        {
            await context.Lots.AddAsync(
                new Lot()
                {
                    
                });
            await context.SaveChangesAsync();
        }

        public async Task PlaceBid(ulong lotId, ulong userId, ulong amount, ulong? maxAmount)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UId == userId);
            if (user == null)
            {
                return;
            }

            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            DateTime time = DateTime.Now;
            if (lot == null || time < lot.LStartTime || time > lot.LFinishTime || lot.LSellerId == userId)
            {
                return;
            }

            IAuctionActions auction = AuctionFactory.GetAuction(lot.LAuctionType);
            var lastBid = await FindLastLotBid(lotId);
            if (auction.BidCheck(lot, amount, time, lastBid))
            {
                Bid? bid = new()
                {
                    BLot = lot,
                    BParticipant = user,
                    BSize = amount,
                    BTime = time
                };
                await context.Bids.AddAsync(bid);
                //отправить сообщение о ставке

                //есть шанс неправильной работы при почти одновременном добавлении автоматических ставок, но уже голова болит об этом думать
                var autoBid = await auction.AutomaticBid(lot, bid, maxAmount, context);
                Bid? prevBid = null;
                while (autoBid != null)
                {
                    prevBid = autoBid;
                    autoBid = await auction.AutomaticBid(lot, prevBid, null, context);
                }
                if (prevBid != null && auction.BidCheck(lot, prevBid.BSize, prevBid.BTime, await FindLastLotBid(lotId)))
                {
                    await context.Bids.AddAsync(prevBid);
                    //отправить сообщение о ставке
                }

                await context.SaveChangesAsync();
            }
        }

        private async Task<Bid?> FindLastLotBid(ulong lotId)
        {
            return await context.Bids
                .Where(b => b.BLotId == lotId)
                .OrderByDescending(b => b.BTime)
                .FirstOrDefaultAsync();
        }
    }
}
