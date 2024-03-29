namespace AuctionWebApp.Server.Interfaces
{
    public interface IAuctionService
    {
        public Task PlaceBid(ulong lotId, ulong userId, ulong amount, ulong? maxAmount);
    }
}
