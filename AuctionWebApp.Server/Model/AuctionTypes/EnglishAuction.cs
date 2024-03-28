using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AuctionWebApp.Server.Model.AuctionTypes
{
    public class EnglishAuction : IAuctionActions
    {
        public bool BidCheck(Lot lot, ulong amount, DateTime time, Bid? lastBid)
        {
            var lastAmount = (lastBid == null) ? lot.LInitialCost : lastBid.BSize;
            return amount >= lastAmount + lot.LCostStep;
        }

        public async Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, ulong? maxBid, MySqlContext context)
        {
            if (lastBid == null)
            {
                return null;
            }

            var lotAutomatic = await context.TrackableLots
                .Where(tl => tl.TlLotId == lot.LId && tl.TlUserId != lastBid.BParticipantId)
                .OrderByDescending(tl => tl.TlMaxAutomaticBid)
                .ToListAsync();

            if (lotAutomatic == null || lotAutomatic[0] == null)
            {
                return null;
            }

            ulong amount = lastBid.BSize + lot.LCostStep;
            Bid? bid = null;
            if (maxBid == null)
            {
                if (lotAutomatic[0].TlMaxAutomaticBid >= amount)
                {
                    bid = new()
                    {
                        BLot = lot,
                        BParticipant = lotAutomatic[0].TlUser,
                        BSize = amount,
                        BTime = DateTime.Now
                    };
                }
            }
            else
            {
                //сообщение о принятии автоставки
                await context.TrackableLots
                    .AddAsync(
                    new()
                    {
                        TlLot = lot,
                        TlUser = lastBid.BParticipant,
                        TlMaxAutomaticBid = maxBid.Value
                    });

                var autoTimes = ((BigInteger)lotAutomatic[0].TlMaxAutomaticBid - lastBid.BSize) / lot.LCostStep;
                autoTimes = (autoTimes.IsEven) ? autoTimes-- : autoTimes;
                var newTimes = ((BigInteger)maxBid - lastBid.BSize) / lot.LCostStep;
                newTimes = (!autoTimes.IsEven) ? newTimes : newTimes--;
                var times = (autoTimes < newTimes) ? autoTimes : newTimes;
                var user = (autoTimes < newTimes) ? lastBid.BParticipant : lotAutomatic[0].TlUser;
                if (times > 0)
                {
                    amount = lastBid.BSize + lot.LCostStep * ((ulong)times + 1);
                    bid = new()
                    {
                        BLot = lot,
                        BParticipant = user,
                        BSize = amount,
                        BTime = DateTime.Now
                    };
                }
            }

            foreach (var item in lotAutomatic)
            {
                if (item.TlMaxAutomaticBid < amount + lot.LCostStep)
                {
                    //уведомить владельца о окончании автоматического повышения ставки
                    context.TrackableLots.Remove(item);
                }
            }

            return bid;
        }
    }
}
