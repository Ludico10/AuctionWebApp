using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionService
    {
        public Task PlaceBid(ulong lotId, ulong userId, ulong amount, ulong? maxAmount);
        public Task<List<Lot>> GetLotsPage(int pageNumber, int lotsOnPage, ushort category);
        public Task<ulong> GetActualCost(Lot lot);
        public Task<LotInfo?> GetLotInfo(ulong lotId);
        public Task PlaceLot(LotInfo lotInfo);
        public Task ChangeLot(LotInfo lotInfo);
        public Task RemoveLot(ulong lotId, ulong winnerId, ulong cost);
    }
}
