using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class AuctionService(MySqlContext context) : IAuctionService
    {
        public async Task PlaceLot(LotInfo lotInfo)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UId == lotInfo.SellerId);
            var condition = await context.ItemConditions.SingleOrDefaultAsync(ic => ic.IcId == lotInfo.ConditionId);
            var type = await context.AuctionTypes.SingleOrDefaultAsync(at => at.AtId == lotInfo.AuctionTypeId);
            if (user == null || condition == null || type == null) 
            {
                return;
            }

            Lot? lot = new(lotInfo, user, type, condition);
            await context.AddAsync(lot);
            await context.SaveChangesAsync();
            lot = context.Lots.SingleOrDefault(l => l.LName == lotInfo.Name
                                                 && l.LSellerId == lotInfo.SellerId);  
            if (lot == null)
            {
                return;
            }

            foreach (var param in lotInfo.Parameters)
            {
                await context.LotAdditionalParameters
                            .AddAsync(
                                new LotAdditionalParameter()
                                {
                                    LapLotId = lot.LId,
                                    LapLot = lot,
                                    LapName = param.Key,
                                    LapValue = param.Value
                                }
                            );
            }

            foreach (var delivery in lotInfo.DeliveryInfos)
            {
                await context.CountryDeliveries.AddAsync(new CountryDelivery(lot.LId, delivery));
            }

            await context.SaveChangesAsync();
        }

        public async Task ChangeLot(LotInfo lotInfo)
        {
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotInfo.Id);
            var condition = await context.ItemConditions.SingleOrDefaultAsync(ic => ic.IcId == lotInfo.ConditionId);
            if (lotInfo.Id == null || lot == null || condition == null)
            {
                return;
            }

            if (await GetActualCost(lot) != lot.LInitialCost)
            {
                return;
            }

            lot.FromLotInfo(lotInfo, condition);
            context.Update(lot);

            foreach (var info in lotInfo.Parameters)
            {
                var param = await context.LotAdditionalParameters.SingleOrDefaultAsync(lap => lap.LapLotId == lot.LId
                                                                                           && lap.LapName == info.Key);
                if (param != null)
                {
                    param.LapValue = info.Value;
                    context.LotAdditionalParameters.Update(param);
                }
            }

            var deliveries = await context.CountryDeliveries
                                          .Where(cd => cd.CdLotId == lot.LId)
                                          .ToListAsync();
            if (deliveries == null)
            {
                return;
            }

            int i = 0;
            var deliveryList = new List<DeliveryInfo>(lotInfo.DeliveryInfos);
            while (deliveries.Count > i)
            {
                var info = deliveryList.SingleOrDefault(di => di.CountryId == deliveries[i].CdCountryId);
                if (info != null)
                {
                    deliveries[i].CdSize = info.Size;
                    context.CountryDeliveries.Update(deliveries[i]);
                    deliveryList.Remove(info);
                    i++;
                }
                else
                {
                    context.CountryDeliveries.Remove(deliveries[i]);
                    deliveries.RemoveAt(i);
                }
            }

            foreach(var delivery in deliveryList)
            {
                await context.CountryDeliveries.AddAsync(new CountryDelivery(lot.LId, delivery));
            }

            await context.SaveChangesAsync();
        }

        public async Task RemoveLot(ulong lotId, ulong winnerId, ulong cost)
        {
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            var winner = await context.Users.SingleOrDefaultAsync(u => u.UId == winnerId);
            if (lot == null || winner == null)
            {
                return;
            }

            await context.FinishedAuctions.AddAsync(new FinishedAuction(lot, winnerId, cost));
            await context.SaveChangesAsync();
            context.Remove(lot);
            //уведомления
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
            if (await auction.BidCheck(lot, amount, time, context))
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
                if (prevBid != null && await auction.BidCheck(lot, prevBid.BSize, prevBid.BTime, context))
                {
                    await context.Bids.AddAsync(prevBid);
                    //отправить сообщение о ставке
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Lot>> GetLotsPage(int pageNumber, int lotsOnPage, ushort category)
        {
            var lots = new List<Lot>();
            var time = DateTime.Now;
            if (pageNumber == 1)
            {
                var date = DateOnly.FromDateTime(time);
                var premiumCategoryLots = await context.LotCategories
                                            .Where(lc => lc.LcCategoryId == category
                                                    && lc.LcPremiumStart != null
                                                    && lc.LcPremiumEnd != null
                                                    && lc.LcPremiumStart <= date
                                                    && lc.LcPremiumEnd >= date)
                                            .OrderBy(lc => lc.LcPremiumEnd)
                                            .ToListAsync();
                if (premiumCategoryLots != null)
                    foreach (var categoryLot in premiumCategoryLots)
                    {
                        var premiumLot = await context.Lots
                                                .Where(l => l.LId == categoryLot.LcLotId
                                                            && l.LFinishTime >= time)
                                                .FirstAsync();
                        lots.Add(premiumLot);
                    }
            }

            var usualLots = await context.Lots
                                    .Where(l => l.LotCategories.Any(lc => lc.LcCategoryId == category)
                                                && l.LFinishTime >= time
                                                && lots.Contains(l))
                                    .OrderBy(l => l.LFinishTime)
                                    .Skip((pageNumber - 1) * lotsOnPage)
                                    .Take(lotsOnPage - lots.Count)
                                    .ToListAsync();
            if (usualLots != null)
            {
                lots.AddRange(usualLots);
            }

            return lots;
        }

        public async Task<LotInfo?> GetLotInfo(ulong lotId)
        {
            var lot = await context.Lots.FirstOrDefaultAsync(l => l.LId == lotId);
            if (lot != null)
            {
                return new LotInfo(lot);
            }

            return null;
        }

        public async Task<ulong> GetActualCost(Lot lot)
        {
            IAuctionActions auction = AuctionFactory.GetAuction(lot.LAuctionType);
            return await auction.GetActualCost(lot, context);
        }
    }
}
