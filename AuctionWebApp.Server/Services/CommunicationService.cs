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

        public async Task<List<SectionShort>> GetNewsNames()
        {
            var result = new List<SectionShort>();
            var sections = await context.InformationSections.ToListAsync();
            if (sections != null)
            {
                foreach (var section in sections)
                {
                    result.Add(new SectionShort(section));
                }
            }

            return result;
        }

        public async Task<string?> GetNewText(uint sectionId)
        {
            var section = await context.InformationSections.FirstOrDefaultAsync(sec => sec.IsId == sectionId);
            if (section != null)
            {
                return section.IsText;
            }

            return null;
        }
    }
}
