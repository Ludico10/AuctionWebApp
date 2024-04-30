using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class LotShortInfo
    {
        public ulong LotId { get; set; }
        public ulong CurrentCost { get; set; }
        public string Name { get; set; } = null!;
        public byte AuctionTypeId { get; set; }
        public bool Actual { get; set; }

        public LotShortInfo(Lot lot, ulong cost) 
        {
            LotId = lot.LId;
            CurrentCost = cost;
            Name = lot.LName;
            AuctionTypeId = lot.LAuctionType;
            Actual = lot.LStartTime.AddDays(1) >= DateTime.Now;
        }
    }
}
