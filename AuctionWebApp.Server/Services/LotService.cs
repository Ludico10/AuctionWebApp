using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class LotService(MySqlContext context, IAuctionService auctionService) : ILotService
    {
        public async Task<LotInfo?> GetInfo(ulong lotId)
        {
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            if (lot != null)
            {
                var auction = AuctionFactory.GetAuction(lot.LAuctionType);
                var costStep = auction.GetCostStep(lot);
                var info = new LotInfo(lot, costStep);
                //чтобы не показывать категорию "все"
                var categories = info.CategoryInfos.ToList();
                categories.RemoveAll(cat => cat.CategoryId == 0);
                info.CategoryInfos = categories;
                return info;
            }

            return null;
        }

        public async Task<Lot?> Place(LotInfo lotInfo)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UId == lotInfo.SellerId);
            var condition = await context.ItemConditions.SingleOrDefaultAsync(ic => ic.IcId == lotInfo.ConditionId);
            var type = await context.AuctionTypes.SingleOrDefaultAsync(at => at.AtId == lotInfo.AuctionTypeId);
            if (user == null || condition == null || type == null)
            {
                return null;
            }

            Lot? lot = context.Lots.SingleOrDefault(l => l.LName == lotInfo.Name
                                                 && l.LSellerId == lotInfo.SellerId);
            if (lot != null)
            {
                return null;
            }

            lot = new(lotInfo, user, type, condition);
            await context.Lots.AddAsync(lot);
            await context.SaveChangesAsync();
            lot = context.Lots.SingleOrDefault(l => l.LName == lotInfo.Name
                                                 && l.LSellerId == lotInfo.SellerId);

            if(lot == null)
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

            await context.LotCategories.AddAsync(new LotCategory(lot.LId, 0));
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
                    if (categories[i].LcCategoryId > 0)
                    {
                        context.LotCategories.Remove(categories[i]);
                        categories.RemoveAt(i);
                    }
                }
            }

            foreach (var category in categoriesList)
            {
                await context.LotCategories.AddAsync(new LotCategory(lotId, category));
            }
        }

        public async Task<bool> Change(LotInfo lotInfo)
        {
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotInfo.Id);
            var condition = await context.ItemConditions.SingleOrDefaultAsync(ic => ic.IcId == lotInfo.ConditionId);
            if (lotInfo.Id == null || lot == null || condition == null)
            {
                return false;
            }

            if (await auctionService.GetActualCost(lot) != lot.LInitialCost)
            {
                return false;
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
            return true;
        }

        public async Task<bool> Remove(ulong lotId)
        {
            var lot = await context.Lots.SingleOrDefaultAsync(l => l.LId == lotId);
            if (lot == null)
            {
                return false;
            }

            await context.FinishedAuctions.AddAsync(new FinishedAuction(lot, lot.LSellerId, lot.LInitialCost));
            //await context.SaveChangesAsync();
            context.Lots.Remove(lot);
            //уведомления
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<int>> FreePremiumDates(ushort categoryId, int month, int year)
        {
            var result = new List<int>();
            var category = await context.Categories.FirstOrDefaultAsync(c => c.CId == categoryId);
            if (category != null)
            {
                var count = DateTime.DaysInMonth(year, month);
                var firstDay = new DateOnly(year, month, 1);
                var lastDay = new DateOnly(year, month, count);
                var candidats = await context.LotCategories
                                            .Where(lc => lc.LcCategoryId == categoryId
                                                      && lc.LcPremiumStart != null
                                                      && lc.LcPremiumEnd != null
                                                      && lc.LcPremiumStart <= lastDay
                                                      && lc.LcPremiumEnd >= firstDay)
                                            .ToListAsync();
                var limit = category.СPaidPositionsCount;
                for (var i = 1; i <= count; i++)
                {
                    int findCount = 0;
                    for (int j = 0; j < candidats.Count && findCount < limit; j++)
                    {
                        if (candidats[i].LcPremiumStart <= firstDay && candidats[i].LcPremiumEnd >= firstDay)
                        {
                            findCount++;
                        }
                    }

                    if (findCount < limit)
                        result.Add(i);
                    firstDay = firstDay.AddDays(1);
                }
            }

            return result;
        }
    }
}
