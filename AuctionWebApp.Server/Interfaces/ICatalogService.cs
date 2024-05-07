using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ICatalogService
    {
        public Task<Dictionary<byte, string>> GetAuctionTypes();
        public Task<Dictionary<ushort, string>> GetCategories(bool withAll);
        public Task<Dictionary<byte, string>> GetConditions();
        public Dictionary<byte, string> GetSortWays();
        public Task<List<(Lot, ulong)>> GetLotsPage(CatalogRequest catalogInfo);
    }
}
