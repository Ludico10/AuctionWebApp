using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionService
    {
        public Task<bool> PlaceBid(ulong lotId, ulong userId, ulong amount, DateTime time, ulong? maxAmount = null);
        public Task AuctionsClosing();
        public Task<ulong> CloseAuction(Lot auction);
        public Task<List<Lot>> GetLotsPage(int pageNumber, int lotsOnPage, ushort category);
        public Task<ulong> GetActualCost(Lot lot);
        public Task<LotInfo?> GetLotInfo(ulong lotId);
        public Task<Lot?> PlaceLot(LotInfo lotInfo);
        public Task ChangeLot(LotInfo lotInfo);
        public Task RemoveLot(ulong lotId, ulong winnerId, ulong cost);
        public Task<Dictionary<byte, string>> GetAuctionTypes();
        public Task<Dictionary<ushort, string>> GetCategories();
    }
}
