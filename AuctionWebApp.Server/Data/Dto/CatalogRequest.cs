namespace AuctionWebApp.Server.Data.Dto
{
    public class CatalogRequest
    {
        public string SearchString = "";
        public ulong MinPrice;
        public ulong? MaxPrice;
        public byte SelectedSorter;
        public short ItemsOnPage;
        public int PageNumber;
        public short CategoryId;
        public Dictionary<short, string>? Conditions;
        public bool[] CondChecked = [];
    }
}
