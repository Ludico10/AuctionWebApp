using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class AuctionService(MySqlContext context) : IAuctionService
    {
        public async Task<bool> PlaceBid(ulong lotId, ulong userId, ulong amount, DateTime time, ulong? maxAmount = null)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UId == userId);
            if (user == null)
            {
                return false;
            }

            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            if (lot == null || time < lot.LStartTime || time > lot.LFinishTime || lot.LSellerId == userId)
            {
                return false;
            }

            IAuctionActions auction = AuctionFactory.GetAuction(lot.LAuctionType);
            if (await auction.BidCheck(lot, amount, time, context))
            {
                Bid? bid = new()
                {
                    BLotId = lot.LId,
                    BParticipantId = user.UId,
                    BSize = amount,
                    BTime = time
                };
                await context.Bids.AddAsync(bid);
                await context.SaveChangesAsync();
                //отправить сообщение о ставке

                //есть шанс неправильной работы при почти одновременном добавлении автоматических ставок, но уже голова болит об этом думать
                Bid? autoBid = await auction.AutomaticBid(lot, bid, context, maxAmount);
                Bid? prevBid = null;
                while (autoBid != null)
                {
                    prevBid = autoBid;
                    autoBid = await auction.AutomaticBid(lot, prevBid, context);
                }
                if (prevBid != null && await auction.BidCheck(lot, prevBid.BSize, prevBid.BTime, context))
                {
                    await context.Bids.AddAsync(prevBid);
                    //отправить сообщение о ставке
                }

                await context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task AuctionsClosing()
        {
            var finnishedAuctions = await context.Lots
                                                .Where(l => l.LFinishTime <= DateTime.Now)
                                                .ToListAsync();
            if (finnishedAuctions == null)
            {
                return;
            }

            foreach (var auction in finnishedAuctions)
            {
                await CloseAuction(auction);
                //уведомления о завершении
                context.Lots.Remove(auction);
            }

            await context.SaveChangesAsync();
        }

        public async Task<ulong> CloseAuction(Lot auction)
        {
            IAuctionActions actions = AuctionFactory.GetAuction(auction.LAuctionType);
            var winnerBid = await actions.GetActualBid(auction, context);
            ulong resultCost;
            if (winnerBid == null)
            {
                resultCost = auction.LInitialCost;
                await context.FinishedAuctions.AddAsync(new FinishedAuction(auction, auction.LSellerId, resultCost));
                //уведомление
            }
            else
            {
                resultCost = winnerBid.BSize;
                await context.FinishedAuctions.AddAsync(new FinishedAuction(auction, winnerBid.BParticipantId, resultCost));
            }

            return resultCost;
        }

        public async Task<bool> IsTrackable(ulong lotId, ulong userId)
        {
            var trackable = await context.TrackableLots.SingleOrDefaultAsync(tl => tl.TlLotId == lotId && tl.TlUserId == userId);
            return trackable != null;
        }

        public async Task ChangeTrackable(ulong lotId, ulong userId, bool isTrackable)
        {
            var trackable = await context.TrackableLots.SingleOrDefaultAsync(tl => tl.TlLotId == lotId && tl.TlUserId == userId);
            if (isTrackable == (trackable != null))
            {
                return;
            }

            if (isTrackable)
            {
                await context.TrackableLots.AddAsync(
                    new TrackableLot()
                    {
                        TlLotId = lotId,
                        TlUserId = userId,
                        TlMaxAutomaticBid = 0
                    });
            }
            else
            {
                context.TrackableLots.Remove(trackable!);
            }

            await context.SaveChangesAsync();
        }

        public async Task<ulong?> GetActualCost(ulong lotId)
        {
            var lot = await context.Lots.FirstOrDefaultAsync(l => l.LId == lotId);
            return (lot != null) ? await GetActualCost(lot) : null;
        }

        public async Task<ulong> GetActualCost(Lot lot)
        {
            IAuctionActions auction = AuctionFactory.GetAuction(lot.LAuctionType);
            return await auction.GetActualCost(lot, DateTime.Now, context);
        }
    }
}
