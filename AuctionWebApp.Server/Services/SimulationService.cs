using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class SimulationService : ISimulationService
    {
        public async Task<List<User>> FindTestUsers(MySqlContext context)
        {
            return await context.Users
                                .Where(u => u.URole.RId == 3)
                                .Take(3)
                                .ToListAsync();
        }

        public LotInfo Preparation(SimulationInfo simulationInfo, User seller)
        {
            foreach (var user in simulationInfo.Users)
            {
                IAuctionActions auction = AuctionFactory.GetAuction(simulationInfo.AuctionTypeId);
                var bounds = auction.GetSimulationUserBounds(simulationInfo, user);
                user.Q = (double)(bounds.Item1 > bounds.Item2 ? bounds.Item1 : bounds.Item2) / 3;
                user.Q = Math.Max(user.Q, 0);
            }

            return new LotInfo()
            {
                Name = "simulation",
                SellerId = seller.UId,
                FinishTime = DateTime.Now.AddHours(simulationInfo.CyclesCount),
                AuctionTypeId = simulationInfo.AuctionTypeId,
                ConditionId = 1,
                InitialCost = simulationInfo.InitialPrice,
                CostStep = simulationInfo.PriceStep
            };
        }

        public SimulationBidInfo? Process(SimulationInfo simulationInfo, int cycle)
        {
            var users = new List<SimulationUser>(simulationInfo.Users);
            var activeUser = rand.Next(0, simulationInfo.Users.Count());
            var potencialBid = NextGaussian(rand) * users[activeUser].Q + users[activeUser].EstimatedCost;

            if (users[activeUser].Budget < potencialBid)
            {
                return null;
            }

            return new SimulationBidInfo()
            {
                Size = Convert.ToUInt64(potencialBid),
                Cycle = cycle,
                SimulationUserId = users[activeUser].Id
            };
        }

        Random rand = new Random();
        private bool haveNextNextGaussian;
        private double nextNextGaussian;

        private double NextGaussian(Random rand)
        {
            if (haveNextNextGaussian)
            {
                haveNextNextGaussian = false;
                return nextNextGaussian;
            }
            else
            {
                double v1, v2, s;
                do
                {
                    v1 = 2 * rand.NextDouble() - 1;   // between -1.0 and 1.0
                    v2 = 2 * rand.NextDouble() - 1;   // between -1.0 and 1.0
                    s = v1 * v1 + v2 * v2;
                } while (s >= 1 || s == 0);
                double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
                nextNextGaussian = v2 * multiplier;
                haveNextNextGaussian = true;
                return v1 * multiplier;
            }
        }
    }
}
