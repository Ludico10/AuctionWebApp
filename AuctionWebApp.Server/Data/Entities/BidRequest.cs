namespace AuctionWebApp.Server.Data.Entities
{
    public class BidRequest
    {
        public DateTime Time { get; set; }

        public ulong UserId { get; set; }

        public ulong Size { get; set; }

        public ulong LotId { get; set; }
    }
}
