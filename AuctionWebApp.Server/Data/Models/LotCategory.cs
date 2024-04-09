using AuctionWebApp.Server.Data.Dto;

namespace AuctionWebApp.Server.Data.Entities;

public partial class LotCategory
{
    public ulong LcLotId { get; set; }

    public ushort LcCategoryId { get; set; }

    public DateOnly? LcPremiumStart { get; set; }

    public DateOnly? LcPremiumEnd { get; set; }

    public virtual Category LcCategory { get; set; } = null!;

    public virtual Lot LcLot { get; set; } = null!;

    public LotCategory() { }

    public LotCategory(ulong lotId, CategoryInfo categoryInfo)
    {
        LcLotId = lotId;
        LcCategoryId = categoryInfo.CategoryId;
        LcPremiumStart = categoryInfo.PremiumStart;
        LcPremiumEnd = categoryInfo.PremiumEnd;
    }
}
