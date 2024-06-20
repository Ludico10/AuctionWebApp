using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ISimulationService
    {
        public Task<List<User>> FindTestUsers(MySqlContext context);
        public LotInfo Preparation(SimulationInfo simulationInfo, User seller);
        public SimulationBidInfo? Process(SimulationInfo simulationInfo, int cycle);
        public void DetermineWinner(SimulationResult simulationResult, int auType);
    }
}
