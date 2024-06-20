using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ICatalogService
    {
        public Task<Dictionary<byte, string>> GetAuctionTypes();
        public Task<Dictionary<ushort, string>> GetCategories(bool withAll);
        public Task<List<PremiumInfo>> GetPremiumCategories();
        public Task<Dictionary<byte, string>> GetConditions();
        public Task<Dictionary<ushort, string>> GetComplaintReasons();
        public Task SendComplaint(ComplaintRequest complaintRequest);
        public Task<Dictionary<ushort, string>> GetDeliveries();
        public Dictionary<byte, string> GetSortWays();
        public Task<(List<(Lot, ulong)>, int)> GetLotsPage(CatalogRequest catalogInfo);
        public Task<(List<(Bid, ulong)>, int)> GetLotsWithBids(ulong userId, int itemsOnPage, int pageNumber);
        public Task<(List<(TrackableLot, ulong)>, int)> GetTrackableLots(ulong userId, int itemsOnPage, int pageNumber);
        public Task<(List<(Lot, ulong)>, int)> GetUserActualLots(ulong userId, int itemsOnPage, int pageNumber);
        public Task<(List<FinishedAuction>, int)> GetUserFinishedLots(ulong userId, bool isWinner, int itemsOnPage, int pageNumber);
    }
}
