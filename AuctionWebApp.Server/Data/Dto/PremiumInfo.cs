using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class PremiumInfo
    {
        public ushort CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public long Payment {  get; set; }

        public PremiumInfo(Category category)
        {
            CategoryId = category.CId;
            CategoryName = category.CName;
            Payment = category.CPaidPositionCoast;
        }
    }
}
