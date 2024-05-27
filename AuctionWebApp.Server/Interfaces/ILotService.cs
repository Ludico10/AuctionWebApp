using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ILotService
    {
        public Task<LotInfo?> GetInfo(ulong lotId);
        public Task<Lot?> Place(LotInfo lotInfo);
        public Task<bool> Change(LotInfo lotInfo);
        public Task<bool> Remove(ulong lotId);
        public Task<List<int>> FreePremiumDates(ushort categoryId, int month, int year);
    }
}
