using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class LotInfo
    {
        public ulong? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = "";
        public ulong SellerId { get; set; }
        public string SellerName { get; set; } = "";
        public int SellerRating { get; set; }
        public DateTime FinishTime { get; set; }
        public byte AuctionTypeId { get; set; }
        public string AuctionTypeName { get; set; } = "";
        public byte ConditionId { get; set; }
        public string ConditionName { get; set; } = "";
        public ulong InitialCost { get; set; }
        public ulong CostStep { get; set; }

        public Dictionary<string, string?> Parameters { get; set; } = [];
        public IEnumerable<DeliveryInfo> DeliveryInfos { get; set; } = [];
        public IEnumerable<CategoryInfo> CategoryInfos { get; set; } = [];

        public LotInfo() { }

        public LotInfo(Lot lot, ulong step)
        {
            Id = lot.LId;
            Name = lot.LName;
            Description = lot.LDescription;
            SellerId = lot.LSellerId;
            SellerName = lot.LSeller.UName;
            SellerRating = lot.LSeller.URating;
            FinishTime = lot.LFinishTime;
            AuctionTypeId = lot.LAuctionType;
            AuctionTypeName = lot.LAuctionTypeNavigation.AtName;
            ConditionId = lot.LConditionId;
            ConditionName = lot.LCondition.IcName;
            InitialCost = lot.LInitialCost;
            CostStep = step;
            Parameters = lot.LotAdditionalParameters.ToDictionary(lap => lap.LapName, lap => lap.LapValue);
            var delInfos = new List<DeliveryInfo>();
            foreach (var info  in lot.CountryDeliveries)
            {
                delInfos.Add(new DeliveryInfo(info));
            }
            DeliveryInfos = delInfos;

            var catInfos = new List<CategoryInfo>();
            foreach (var info in lot.LotCategories)
            {
                catInfos.Add(new CategoryInfo(info));
            }
            CategoryInfos = catInfos;
        }
    }
}
