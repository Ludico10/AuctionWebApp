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

        public async Task<List<PremiumInfo>> GetPremiumCategories()
        {
            var result = new List<PremiumInfo>();
            var categories = await context.Categories
                                          .Where(c => c.CId > 0)
                                          .ToListAsync();
            foreach (var category in categories) 
            {
                result.Add(new PremiumInfo(category));
            }

            return result;
        }

        public async Task<Dictionary<byte, string>> GetConditions()
        {
            return await context.ItemConditions.ToDictionaryAsync(ic => ic.IcId, ic => ic.IcName);
        }

        public async Task<Dictionary<ushort, string>> GetComplaintReasons()
        {
            return await context.AuctionComplaintReasons.ToDictionaryAsync(acr => acr.AcrId, acr => acr.AcrName);
        }

        public async Task SendComplaint(ComplaintRequest complaintRequest)
        {
            var date = DateOnly.FromDateTime(DateTime.Now);
            var complaint = new AuctionComplaint(complaintRequest, date);
            await context.AuctionComplaints.AddAsync(complaint);
            await context.SaveChangesAsync();
        }

        public async Task<Dictionary<ushort, string>> GetDeliveries()
        {
            return await context.Countries.ToDictionaryAsync(c => c.CouId, c => c.CouName);
        }

        private async Task<List<byte>> GetActualConditions(CatalogRequest catalogInfo)
        {
            var conditionIds = new List<byte>();
            var all = catalogInfo.Conditions is null;
            if (all)
                catalogInfo.Conditions = await GetConditions();

            int i = 0;
            foreach (var cond in catalogInfo.Conditions!)
            {
                if (all || catalogInfo.CondChecked[i])
                    conditionIds.Add(cond.Key);
                i++;
            }

            return conditionIds;
        }

        private async Task<List<byte>> GetActualTypes(CatalogRequest catalogInfo)
        {
            var typeIds = new List<byte>();
            var all = catalogInfo.AuctionTypes is null;
            if (all)
                catalogInfo.AuctionTypes = await GetAuctionTypes();

            int i = 0;
            foreach (var type in catalogInfo.AuctionTypes!)
            {
                if (all || catalogInfo.TypeChecked[i])
                    typeIds.Add(type.Key);
                i++;
            }

            return typeIds;
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

        private List<(Lot, ulong)> SortTakeLots(List<(Lot, ulong)> list, CatalogRequest info, int skiped, int taked)
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
            return order.Skip((info.PageNumber - 1) * info.ItemsOnPage - skiped)
                   .Take(info.ItemsOnPage - taked)
                   .ToList();
        }

        public async Task<(List<(Lot, ulong)>, int)> GetLotsPage(CatalogRequest catalogInfo)
        {
            catalogInfo.MaxPrice ??= ulong.MaxValue;
            var conditions = await GetActualConditions(catalogInfo);
            var types = await GetActualTypes(catalogInfo);
            var lots = new List<(Lot, ulong)>();
            var time = DateTime.Now;
            var date = DateOnly.FromDateTime(time);
            var premiumCategoryLots = new List<LotCategory>();
            premiumCategoryLots = await context.LotCategories
                                            .Where(lc => lc.LcCategoryId == catalogInfo.CategoryId
                                                    && lc.LcPremiumStart != null
                                                    && lc.LcPremiumEnd != null
                                                    && lc.LcPremiumStart <= date
                                                    && lc.LcPremiumEnd >= date)
                                            .ToListAsync();
            foreach (var categoryLot in premiumCategoryLots)
            {
                var premiumLot = await context.Lots
                                           .Where(l => l.LId == categoryLot.LcLotId
                                                    && l.LName.Contains(catalogInfo.SearchString)
                                                    && l.LFinishTime >= time
                                                    && types.Contains(l.LAuctionType)
                                                    && conditions.Contains(l.LConditionId))
                                           .SingleOrDefaultAsync();
                if (premiumLot != null)
                {
                    var cost = await auctionService.GetActualCost(premiumLot);
                    if (cost >= catalogInfo.MinPrice && cost <= catalogInfo.MaxPrice)
                    {
                        lots.Add((premiumLot, cost));
                    }
                }
            }

            int count = lots.Count;
            lots = SortTakeLots(lots, catalogInfo, 0, 0);

            if (lots.Count <= catalogInfo.ItemsOnPage)
            {
                var usualLots = await context.Lots
                                        .Where(l => l.LotCategories.Any(lc => lc.LcCategoryId == catalogInfo.CategoryId)
                                                    && l.LName.Contains(catalogInfo.SearchString)
                                                    && types.Contains(l.LAuctionType)
                                                    && conditions.Contains(l.LConditionId)
                                                    && l.LFinishTime >= time)
                                        .ToListAsync();
                List<(Lot, ulong)> usualLotCosts = [];
                if (usualLots != null)
                {
                    foreach (var usualLot in usualLots)
                    {
                        if (!lots.Any(lot => lot.Item1.LId == usualLot.LId))
                        {
                            var cost = await auctionService.GetActualCost(usualLot);
                            if (cost >= catalogInfo.MinPrice && cost <= catalogInfo.MaxPrice)
                            {
                                usualLotCosts.Add((usualLot, cost));
                            }
                        }
                    }

                    count += usualLots.Count;
                    usualLotCosts = SortTakeLots(usualLotCosts, catalogInfo, premiumCategoryLots.Count, lots.Count);
                }

                lots.AddRange(usualLotCosts);
            }

            return (lots, count);
        }

        public async Task<(List<(Bid, ulong)>, int)> GetLotsWithBids(ulong userId, int itemsOnPage, int pageNumber)
        {
            var result = new List<(Bid, ulong)>();
            int count = 0;
            var bids = await context.Bids
                                    .Where(b => b.BParticipantId == userId)
                                    .OrderByDescending(b => b.BTime)
                                    .ToListAsync();
            if (bids != null)
            {
                int skipCount = (pageNumber - 1) * itemsOnPage;
                int takeCount = 0;
                foreach (var bid in bids)
                {
                    var lot = bid.BLot;
                    if (!result.Any(r => r.Item1.BLot == lot))
                    {
                        count++;
                        if (skipCount <= 0 && takeCount < itemsOnPage)
                        {
                            var cost = await auctionService.GetActualCost(lot);
                            result.Add((bid, cost));
                            takeCount++;
                        }
                        skipCount--;
                    }
                }
            }

            return (result, count);
        }

        public async Task<(List<(TrackableLot, ulong)>, int)> GetTrackableLots(ulong userId, int itemsOnPage, int pageNumber)
        {
            var result = new List<(TrackableLot, ulong)>();
            var trackableLots = await context.TrackableLots
                                             .Where(tl => tl.TlUserId == userId)
                                             .OrderBy(tl => tl.TlLot.LFinishTime)
                                             .ToListAsync();
            var count = trackableLots.Count;
            trackableLots = trackableLots.Skip((pageNumber - 1) * itemsOnPage)
                                         .Take(itemsOnPage)
                                         .ToList();
            if (trackableLots != null)
            {
                foreach (var trackable in trackableLots)
                {
                    var cost = await auctionService.GetActualCost(trackable.TlLot);
                    result.Add((trackable, cost));
                }
            }

            return (result, count);
        }

        public async Task<(List<(Lot, ulong)>, int)> GetUserActualLots(ulong userId, int itemsOnPage, int pageNumber)
        {
            var result = new List<(Lot, ulong)>();
            var lots = await context.Lots
                                    .Where(l => l.LSellerId == userId)
                                    .OrderBy(l => l.LFinishTime)
                                    .Skip((pageNumber - 1) * itemsOnPage)
                                    .Take(itemsOnPage)
                                    .ToListAsync();
            var count = lots.Count;
            lots = lots.Skip((pageNumber - 1) * itemsOnPage)
                       .Take(itemsOnPage)
                       .ToList();
            if (lots != null)
            {
                foreach(var lot in lots)
                {
                    var cost = await auctionService.GetActualCost(lot);
                    result.Add((lot, cost));
                }
            }

            return (result, count);
        }

        public async Task<(List<FinishedAuction>, int)> GetUserFinishedLots(ulong userId, bool isWinner, int itemsOnPage, int pageNumber)
        {
            var result = new List<FinishedAuction>();
            result = await context.FinishedAuctions
                                .Where(fa => (isWinner && fa.FaWinnerId == userId)
                                          || (!isWinner && fa.FaSellerId == userId))
                                .OrderByDescending(fa => fa.FaStateUpdateTime)
                                .ToListAsync();
            var count = result.Count;
            result = result.Skip((pageNumber - 1) * itemsOnPage)
                           .Take(itemsOnPage)
                           .ToList();
            return (result, count);
        }
    }
}
