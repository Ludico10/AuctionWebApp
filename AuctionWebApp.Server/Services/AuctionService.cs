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
        public async Task<Lot?> PlaceLot(LotInfo lotInfo)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UId == lotInfo.SellerId);
            var condition = await context.ItemConditions.SingleOrDefaultAsync(ic => ic.IcId == lotInfo.ConditionId);
            var type = await context.AuctionTypes.SingleOrDefaultAsync(at => at.AtId == lotInfo.AuctionTypeId);
            if (user == null || condition == null || type == null)
            {
                return null;
            }

            Lot? lot = new(lotInfo, user, type, condition);
            await context.AddAsync(lot);
            await context.SaveChangesAsync();
            lot = context.Lots.SingleOrDefault(l => l.LName == lotInfo.Name
                                                 && l.LSellerId == lotInfo.SellerId);
            if (lot == null)
            {
                return lot;
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

            foreach (var category in lotInfo.CategoryInfos)
            {
                await context.LotCategories.AddAsync(new LotCategory(lot.LId, category));
            }

            await context.SaveChangesAsync();
            return lot;
        }

        private async Task ChangeDeliveries(ulong lotId, LotInfo lotInfo)
        {
            var deliveries = await context.CountryDeliveries
                                          .Where(cd => cd.CdLotId == lotId)
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

            foreach (var delivery in deliveryList)
            {
                await context.CountryDeliveries.AddAsync(new CountryDelivery(lotId, delivery));
            }
        }

        private async Task ChangeCategories(ulong lotId, LotInfo lotInfo)
        {
            var categories = await context.LotCategories
                                        .Where(lc => lc.LcLotId == lotId)
                                        .ToListAsync();
            if (categories == null)
            {
                return;
            }

            int i = 0;
            var categoriesList = new List<CategoryInfo>(lotInfo.CategoryInfos);
            while (categories.Count > i)
            {
                var info = categoriesList.SingleOrDefault(ci => ci.CategoryId == categories[i].LcCategoryId);
                if (info != null)
                {
                    categories[i].LcPremiumStart = info.PremiumStart;
                    categories[i].LcPremiumEnd = info.PremiumEnd;
                    context.LotCategories.Update(categories[i]);
                    categoriesList.Remove(info);
                    i++;
                }
                else
                {
                    context.LotCategories.Remove(categories[i]);
                    categories.RemoveAt(i);
                }
            }

            foreach (var category in categoriesList)
            {
                await context.LotCategories.AddAsync(new LotCategory(lotId, category));
            }
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

            await ChangeDeliveries(lot.LId, lotInfo);
            await ChangeCategories(lot.LId, lotInfo);
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
            }
            else
            {
                resultCost = winnerBid.BSize;
                await context.FinishedAuctions.AddAsync(new FinishedAuction(auction, winnerBid.BParticipantId, resultCost));
            }

            return resultCost;
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
                                                && !lots.Contains(l))
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
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            if (lot != null)
            {
                return new LotInfo(lot);
            }

            return null;
        }

        public async Task<ulong> GetActualCost(Lot lot)
        {
            IAuctionActions auction = AuctionFactory.GetAuction(lot.LAuctionType);
            return await auction.GetActualCost(lot, DateTime.Now, context);
        }

        public async Task<Dictionary<byte, string>> GetAuctionTypes()
        {
            return await context.AuctionTypes.ToDictionaryAsync(at => at.AtId, at => at.AtName);
        }

        public async Task<Dictionary<ushort, string>> GetCategories()
        {
            return await context.Categories.ToDictionaryAsync(c => c.CId, c => c.CName);
        }
    }
}
