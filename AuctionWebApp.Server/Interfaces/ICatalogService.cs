using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ICatalogService
    {
        public Task<Dictionary<byte, string>> GetAuctionTypes();
        public Task<Dictionary<ushort, string>> GetCategories(bool withAll);
        public Task<Dictionary<byte, string>> GetConditions();
        public Task<Dictionary<ushort, string>> GetDeliveries();
        public Dictionary<byte, string> GetSortWays();
        public Task<(List<(Lot, ulong)>, int)> GetLotsPage(CatalogRequest catalogInfo);
        public Task<List<(Bid, ulong)>> GetLotsWithBids(ulong userId, int itemsOnPage, int pageNumber);
        public Task<List<(TrackableLot, ulong)>> GetTrackableLots(ulong userId, int itemsOnPage, int pageNumber);
        public Task<List<(Lot, ulong)>> GetUserActualLots(ulong userId, int itemsOnPage, int pageNumber);
        public Task<List<FinishedAuction>> GetUserFinishedLots(ulong userId, bool isWinner, int itemsOnPage, int pageNumber);
    }
}
