using AuctionWebApp.Server.Data.Dto;

namespace AuctionWebApp.Server.Interfaces
{
    public interface ICommunicationService
    {
        public Task<List<CommentInfo>> GetLotComments(ulong lotId);
        public Task PlaceComment(CommentInfo comment, ulong lotId);
        public Task<List<SectionShort>> GetNewsNames();
        public Task<string?> GetNewText(uint sectionId);
    }
}
