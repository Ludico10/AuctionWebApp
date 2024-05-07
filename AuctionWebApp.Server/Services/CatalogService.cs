using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class CatalogService(MySqlContext context, IAuctionService auctionService) : ICatalogService
    {
        public async Task<Dictionary<byte, string>> GetAuctionTypes()
        {
            return await context.AuctionTypes.ToDictionaryAsync(at => at.AtId, at => at.AtName);
        }

        public async Task<Dictionary<ushort, string>> GetCategories(bool withAll)
        {
            return await context.Categories
                .Where(c => withAll || c.CId > 0)
                .ToDictionaryAsync(c => c.CId, c => c.CName);
        }

        public async Task<Dictionary<byte, string>> GetConditions()
        {
            return await context.ItemConditions.ToDictionaryAsync(ic => ic.IcId, ic => ic.IcName);
        }

        private async Task<List<short>> GetActualConditions(CatalogRequest catalogInfo)
        {
            var conditionIds = new List<short>();
            if (catalogInfo.Conditions != null)
            {
                int i = 0;
                foreach (var cond in catalogInfo.Conditions)
                {
                    if (catalogInfo.CondChecked[i])
                        conditionIds.Add(cond.Key);
                    i++;
                }

                return conditionIds;
            }

            var conditions = await context.ItemConditions.ToListAsync();
            foreach (var cond in conditions)
            {
                conditionIds.Add(cond.IcId);
            }

            return conditionIds;
        }

        public Dictionary<byte, string> GetSortWays()
        {
            return new Dictionary<byte, string>()
            {
                { 1, "по возрастанию остатка времени" },
                { 2, "по убыванию остатка времени" },
                { 3, "по возрастанию текущей цены" },
                { 4, "по убыванию текущей цены" },
                { 5, "по возрастанию времени размещения" },
                { 6, "по убыванию времени размещения" }
            };
        }

        private List<(Lot, ulong)> SortTakeLots(List<(Lot, ulong)> list, CatalogRequest info, int count)
        {
            IOrderedEnumerable<(Lot, ulong)>? order = null;
            order = info.SelectedSorter switch
            {
                1 => list.OrderBy(l => l.Item1.LFinishTime),
                2 => list.OrderByDescending(l => l.Item1.LFinishTime),
                3 => list.OrderBy(l => l.Item2),
                4 => list.OrderByDescending(l => l.Item2),
                6 => list.OrderByDescending(l => l.Item1.LStartTime),
                _ => list.OrderBy(l => l.Item1.LStartTime),
            };
            return order.Skip((info.PageNumber - 1) * info.ItemsOnPage)
                   .Take(count)
                   .ToList();
        }

        public async Task<List<(Lot, ulong)>> GetLotsPage(CatalogRequest catalogInfo)
        {
            catalogInfo.MaxPrice ??= ulong.MaxValue;
            var conditions = await GetActualConditions(catalogInfo);
            var lots = new List<(Lot, ulong)>();
            var time = DateTime.Now;
            var date = DateOnly.FromDateTime(time);
            var premiumCategoryLots = await context.LotCategories
                                            .Where(lc => lc.LcCategoryId == catalogInfo.CategoryId
                                                    && lc.LcPremiumStart != null
                                                    && lc.LcPremiumEnd != null
                                                    && lc.LcPremiumStart <= date
                                                    && lc.LcPremiumEnd >= date)
                                            .ToListAsync();
            if (premiumCategoryLots != null && premiumCategoryLots.Count > (catalogInfo.PageNumber - 1) * catalogInfo.ItemsOnPage)
            {
                foreach (var categoryLot in premiumCategoryLots)
                {
                    var premiumLot = await context.Lots
                                           .Where(l => l.LId == categoryLot.LcLotId
                                                    && l.LName.Contains(catalogInfo.SearchString)
                                                    && l.LFinishTime >= time
                                                    && conditions.Contains(l.LConditionId))
                                           .FirstAsync();
                    var cost = await auctionService.GetActualCost(premiumLot);
                    if (cost >= catalogInfo.MinPrice && cost <= catalogInfo.MaxPrice)
                    {
                        lots.Add((premiumLot, cost));
                    }
                }

                lots = SortTakeLots(lots, catalogInfo, catalogInfo.ItemsOnPage);
            }

            if (lots.Count <= catalogInfo.ItemsOnPage)
            {
                var usualLots = await context.Lots
                                        .Where(l => l.LotCategories.Any(lc => lc.LcCategoryId == catalogInfo.CategoryId)
                                                    && !lots.Any(lot => lot.Item1.LId == l.LId)
                                                    && l.LName.Contains(catalogInfo.SearchString)
                                                    && conditions.Contains(l.LConditionId)
                                                    && l.LFinishTime >= time)
                                        .ToListAsync();
                List<(Lot, ulong)> usualLotCosts = [];
                if (usualLots != null)
                {
                    foreach (var usualLot in usualLots)
                    {
                        var cost = await auctionService.GetActualCost(usualLot);
                        if (cost >= catalogInfo.MinPrice && cost <= catalogInfo.MaxPrice)
                        {
                            usualLotCosts.Add((usualLot, cost));
                        }
                    }

                    usualLotCosts = SortTakeLots(usualLotCosts, catalogInfo, catalogInfo.ItemsOnPage - lots.Count);
                }

                lots.AddRange(usualLotCosts);
            }

            return lots;
        }
    }
}
