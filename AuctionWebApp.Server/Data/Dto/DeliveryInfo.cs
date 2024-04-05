using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class DeliveryInfo
    {
        public ushort CountryId { get; set; }
        public string CountryName { get; set; } = null!;
        public uint Size { get; set; }

        public DeliveryInfo(CountryDelivery countryDelivery)
        {
            CountryId = countryDelivery.CdCountryId;
            CountryName = countryDelivery.CdCountry.CouName;
            Size = countryDelivery.CdSize;
        }
    }
}
