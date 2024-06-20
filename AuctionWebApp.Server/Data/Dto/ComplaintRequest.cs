namespace AuctionWebApp.Server.Data.Dto
{
    public class ComplaintRequest
    {
        public ushort ReasonId { get; set; }
        public ulong LotId { get; set; }
        public string Comment { get; set; } = "";
    }
}
