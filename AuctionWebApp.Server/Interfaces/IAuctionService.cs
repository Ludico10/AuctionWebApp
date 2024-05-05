using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionService
    {
        public Task<bool> PlaceBid(ulong lotId, ulong userId, ulong amount, DateTime time, ulong? maxAmount = null);
        public Task<ulong?> GetActualCost(ulong lotId);
        public Task AuctionsClosing();
        public Task<ulong> CloseAuction(Lot auction);
        public Dictionary<byte, string> GetSortWays();
        public Task<List<(Lot, ulong)>> GetLotsPage(CatalogRequest catalogInfo);
        public Task<ulong> GetActualCost(Lot lot);
        public Task<LotInfo?> GetLotInfo(ulong lotId);
        public Task<Lot?> PlaceLot(LotInfo lotInfo);
        public Task ChangeLot(LotInfo lotInfo);
        public Task RemoveLot(ulong lotId, ulong winnerId, ulong cost);
        public Task<List<CommentInfo>> GetLotComments(ulong lotId);
        public Task PlaceComment(CommentInfo comment, ulong lotId);
        public Task<bool> GetTrackable(ulong lotId, ulong userId);
        public Task ChangeTrackable(ulong lotId, ulong userId, bool isTrackable);
        public Task<Dictionary<byte, string>> GetAuctionTypes();
        public Task<Dictionary<ushort, string>> GetCategories(bool withAll);
        public Task<Dictionary<byte, string>> GetConditions();
    }
}
