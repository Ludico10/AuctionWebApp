using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ILotService
    {
        public Task<LotInfo?> GetInfo(ulong lotId);
        public Task<Lot?> Place(LotInfo lotInfo);
        public Task Change(LotInfo lotInfo);
        public Task Remove(ulong lotId, ulong winnerId, ulong cost);
    }
}
