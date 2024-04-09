namespace AuctionWebApp.Server.Data.Dto
{
    public class SimulationUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = "user";
        public ulong EstimatedCost { get; set; }
        public ulong Budget { get; set; }
        public double BetProbabilityBefore { get; set; }
        public double BetProbabilityAfter { get; set; }
        public double Q { get; set; }
    }
}
