using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model.AuctionTypes;

namespace AuctionWebApp.Server.Model
{
    public class AuctionFactory
    {
        public static IAuctionActions GetAuction(byte typeId)
        {
            return new EnglishAuction();
        }
    }
}
