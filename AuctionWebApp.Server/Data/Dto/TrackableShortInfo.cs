namespace AuctionWebApp.Server.Data.Dto
{
    public class TrackableShortInfo
    {
        public LotShortInfo LotInfo { get; set; }
        public ulong? AutomaticBid { get; set; }

        public TrackableShortInfo(LotShortInfo lotShort, ulong? automaticBid)
        {
            LotInfo = lotShort;
            AutomaticBid = (automaticBid != null && automaticBid == 0) ? null : automaticBid;
        }
    }
}
