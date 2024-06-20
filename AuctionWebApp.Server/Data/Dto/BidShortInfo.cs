using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class BidShortInfo
    {
        public LotShortInfo LotInfo { get; set; }
        public ulong Size { get; set; }
        public DateTime Time { get; set; }

        public BidShortInfo(LotShortInfo lotInfo, Bid bid)
        {
            LotInfo = lotInfo;
            Size = bid.BSize;
            Time = bid.BTime;
        }

        public BidShortInfo(LotShortInfo lotInfo)
        {
            LotInfo = lotInfo;
            Time = DateTime.Now;
        }
    }
}
