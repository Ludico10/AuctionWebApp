using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class FinishedShortInfo
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = "";
        public ulong Cost { get; set; }
        public string State { get; set; } = "";

        public FinishedShortInfo(FinishedAuction auction)
        {
            Id = auction.FaId;
            Name = auction.FaName;
            Cost = auction.FaSize;
            State = auction.FaCurrentState.SName;
        }

        public FinishedShortInfo(ulong count)
        {
            Cost = count;
        }
    }
}
