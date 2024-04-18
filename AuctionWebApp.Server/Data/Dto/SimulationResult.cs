namespace AuctionWebApp.Server.Data.Dto
{
    public class SimulationResult
    {
        public int? WinnerId { get; set; }
        public ulong ResultCost { get; set; }
        public IEnumerable<SimulationBidInfo> Bids { get; set; } = [];
    }
}
