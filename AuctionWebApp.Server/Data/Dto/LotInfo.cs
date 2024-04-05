using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class LotInfo
    {
        public ulong? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ulong SellerId { get; set; }
        public DateTime FinishTime { get; set; }
        public byte AuctionTypeId { get; set; }
        public string AuctionTypeName { get; set; }
        public byte ConditionId { get; set; }
        public string ConditionName { get; set; }
        public ulong InitialCost { get; set; }
        public ulong CostStep { get; set; }

        public Dictionary<string, string?> Parameters { get; set; }
        public IEnumerable<DeliveryInfo> DeliveryInfos { get; set; }

        public LotInfo(Lot lot)
        {
            Id = lot.LId;
            Name = lot.LName;
            Description = lot.LDescription;
            SellerId = lot.LSellerId;
            FinishTime = lot.LFinishTime;
            AuctionTypeId = lot.LAuctionType;
            AuctionTypeName = lot.LAuctionTypeNavigation.AtName;
            ConditionId = lot.LConditionId;
            ConditionName = lot.LCondition.IcName;
            InitialCost = lot.LInitialCost;
            CostStep = lot.LCostStep;
            Parameters = lot.LotAdditionalParameters.ToDictionary(lap => lap.LapName, lap => lap.LapValue);
            var infos = new List<DeliveryInfo>();
            foreach (var info  in lot.CountryDeliveries)
            {
                infos.Add(new DeliveryInfo(info));
            }
            DeliveryInfos = infos;
        }
    }
}
