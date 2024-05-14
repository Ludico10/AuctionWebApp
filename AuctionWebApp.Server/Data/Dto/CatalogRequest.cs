namespace AuctionWebApp.Server.Data.Dto
{
    public class CatalogRequest
    {
        public ulong? UserId { get; set; }
        public string SearchString { get; set; } = "";
        public ulong MinPrice { get; set; }
        public ulong? MaxPrice { get; set; }
        public byte SelectedSorter { get; set; }
        public short ItemsOnPage { get; set; }
        public int PageNumber { get; set; }
        public short CategoryId { get; set; }
        public Dictionary<byte, string>? AuctionTypes { get; set; }
        public bool[] TypeChecked { get; set; } = [];
        public Dictionary<byte, string>? Conditions { get; set; }
        public bool[] CondChecked { get; set; } = [];
    }
}
