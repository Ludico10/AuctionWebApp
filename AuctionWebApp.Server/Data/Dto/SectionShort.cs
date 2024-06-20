using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class SectionShort
    {
        public uint Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public uint? ParentSectionId { get; set; }

        public SectionShort() { }

        public SectionShort(InformationSection section)
        {
            Id = section.IsId;
            Name = section.IsName;
            ParentSectionId = section.IsParentSectionId;
        }
    }
}
