using AuctionWebApp.Server.Data.Dto;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Lot
{
    public ulong LId { get; set; }

    public string LName { get; set; } = null!;

    public string LDescription { get; set; } = null!;

    public ulong LSellerId { get; set; }

    public DateTime LStartTime { get; set; }

    public DateTime LFinishTime { get; set; }

    public byte LAuctionType { get; set; }

    public byte LConditionId { get; set; }

    public sbyte LRatingRestriction { get; set; }

    public ulong LInitialCost { get; set; }

    public ulong LCostStep { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = [];

    public virtual ICollection<Comment> Comments { get; set; } = [];

    public virtual ICollection<CountryDelivery> CountryDeliveries { get; set; } = [];

    public virtual AuctionType LAuctionTypeNavigation { get; set; } = null!;

    public virtual ItemCondition LCondition { get; set; } = null!;

    public virtual User LSeller { get; set; } = null!;

    public virtual ICollection<LotAdditionalParameter> LotAdditionalParameters { get; set; } = [];

    public virtual ICollection<LotCategory> LotCategories { get; set; } = [];

    public virtual ICollection<TrackableLot> TrackableLots { get; set; } = [];

    public void FromLotInfo(LotInfo lotInfo, ItemCondition condition)
    {
        LName = lotInfo.Name;
        LDescription = lotInfo.Description;
        LFinishTime = lotInfo.FinishTime;
        LConditionId = lotInfo.ConditionId;
        LCondition = condition;
        LInitialCost = lotInfo.InitialCost;
        LCostStep = lotInfo.CostStep;
    }

    public Lot(LotInfo lotInfo, User user, AuctionType auctionType, ItemCondition condition)
    {
        FromLotInfo(lotInfo, condition);
        LStartTime = DateTime.Now;
        LSellerId = user.UId;
        LSeller = user;
        LAuctionType = auctionType.AtId;
        LAuctionTypeNavigation = auctionType;
    }

    public Lot() { }
}
