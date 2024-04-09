using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class CategoryInfo
    {
        public ushort CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateOnly? PremiumStart { get; set; }
        public DateOnly? PremiumEnd { get; set; }

        public CategoryInfo(LotCategory lotCategory) 
        { 
            CategoryId = lotCategory.LcCategoryId;
            CategoryName = lotCategory.LcCategory.CName;
            PremiumStart = lotCategory.LcPremiumStart;
            PremiumEnd = lotCategory.LcPremiumEnd;
        }
    }
}
