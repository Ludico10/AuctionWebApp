using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class CommunicationService(MySqlContext context) : ICommunicationService
    {
        public async Task<List<CommentInfo>> GetLotComments(ulong lotId)
        {
            var comments = await context.Comments
                                        .Where(c => c.ComLotId == lotId)
                                        .OrderBy(c => c.ComTime)
                                        .ToListAsync();
            var result = new List<CommentInfo>();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    result.Add(new CommentInfo(comment));
                }
            }

            return result;
        }

        public async Task PlaceComment(CommentInfo comment, ulong lotId)
        {
            await context.Comments.AddAsync(new Comment(comment, lotId));
            await context.SaveChangesAsync();
        }
    }
}
