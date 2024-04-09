using AuctionWebApp.Server.Interfaces;
using AuctionWebApp.Server.Model.AuctionTypes;

namespace AuctionWebApp.Server.Model
{
    public class AuctionFactory
    {
        public static IAuctionActions GetAuction(byte typeId)
        {
            switch (typeId)
            {
                case 2: return new DutchAuction();
                case 3: return new HiddenAuction();
                case 4: return new HiddenSecondPriceAuction();
                default: return new EnglishAuction();
            }
        }
    }
}
