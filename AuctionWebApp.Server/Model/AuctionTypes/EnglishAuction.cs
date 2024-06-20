using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace AuctionWebApp.Server.Model.AuctionTypes
{
    public class EnglishAuction : IAuctionActions
    {
        public async Task<Bid?> GetActualBid(Lot lot, MySqlContext context)
        {
            return await context.Bids
                            .Where(b => b.BLotId == lot.LId)
                            .OrderByDescending(b => b.BSize)
                            .ThenBy(b => b.BTime)
                            .FirstOrDefaultAsync();
        }

        public async Task<ulong> GetActualCost(Lot lot, DateTime time, MySqlContext context)
        {
            var lastBid = await GetActualBid(lot, context);
            if (lastBid == null)
            {
                return lot.LInitialCost;
            }

            return lastBid.BSize;
        }

        public async Task<bool> BidCheck(Lot lot, ulong amount, DateTime time, MySqlContext context)
        {
            var actualCost = await GetActualCost(lot, time, context) + lot.LCostStep;
            return amount >= actualCost;
        }

        private async Task<TrackableLot> PlaceAutobid(MySqlContext context, ulong lotId, ulong userId, ulong amount)
        {
            var trackable = await context.TrackableLots.SingleOrDefaultAsync(tl => tl.TlLotId == lotId && tl.TlUserId == userId);
            if (trackable != null)
            {
                trackable.TlMaxAutomaticBid = amount;
                return trackable;
            }

            trackable = new TrackableLot()
            {
                TlLotId = lotId,
                TlUserId = userId,
                TlMaxAutomaticBid = amount
            };
            //сообщение о принятии автоставки
            await context.TrackableLots.AddAsync(trackable);
            return trackable;
        }

        public async Task<Bid?> AutomaticBid(Lot lot, Bid? lastBid, MySqlContext context, ulong? maxBid = null)
        {
            if (lastBid == null)
            {
                return null;
            }

            var lotAutomatic = await context.TrackableLots
                .Where(tl => tl.TlLotId == lot.LId && tl.TlUserId != lastBid.BParticipant.UId && tl.TlMaxAutomaticBid != null)
                .OrderByDescending(tl => tl.TlMaxAutomaticBid)
                .ToListAsync();

            ulong amount = lastBid.BSize + lot.LCostStep;
            Bid? bid = null;
            if (maxBid == null)
            {
                if (lotAutomatic == null || lotAutomatic.Count == 0)
                {
                    return bid;
                }

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
                var trackable = await PlaceAutobid(context, lot.LId, lastBid.BParticipantId, maxBid.Value);
                if (lotAutomatic == null || lotAutomatic.Count == 0)
                {
                    return null;
                }

                var autoTimes = ((BigInteger)lotAutomatic[0].TlMaxAutomaticBid! - lastBid.BSize) / lot.LCostStep;
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

                lotAutomatic.Add(trackable);
            }

            foreach (var item in lotAutomatic)
            {
                if (item.TlMaxAutomaticBid < amount + lot.LCostStep)
                {
                    //уведомить владельца о окончании автоматического повышения ставки
                    //context.TrackableLots.Remove(item);
                    item.TlMaxAutomaticBid = 0;
                }
            }

            return bid;
        }

        public ulong GetCostStep(Lot lot) 
        { 
            return lot.LCostStep; 
        }

        public (BigInteger, BigInteger) GetSimulationUserBounds(SimulationInfo simulationInfo, SimulationUser user)
        {
            var lowerBound = (BigInteger)user.EstimatedCost - simulationInfo.InitialPrice;
            var upperBound = (BigInteger)user.Budget - user.EstimatedCost;
            return (lowerBound, upperBound);
        }
    }
}
