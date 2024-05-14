using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionService
    {
        public Task<bool> PlaceBid(ulong lotId, ulong userId, ulong amount, DateTime time, ulong? maxAmount = null);
        public Task<ulong?> GetActualCost(ulong lotId);
        public Task<ulong> GetActualCost(Lot lot);
        public Task AuctionsClosing();
        public Task<ulong> CloseAuction(Lot auction);
        public Task<bool> IsTrackable(ulong lotId, ulong userId);
        public Task ChangeTrackable(ulong lotId, ulong userId, bool isTrackable);
    }
}
