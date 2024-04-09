namespace AuctionWebApp.Server.Data.Dto
{
    public class SimulationInfo
    {
        public ulong InitialPrice { get; set; }
        public ulong PriceStep { get; set; }
        public int CyclesCount { get; set; }
        public byte AuctionTypeId { get; set; }
        public IEnumerable<SimulationUser> Users { get; set; } = new List<SimulationUser>();
    }
}
