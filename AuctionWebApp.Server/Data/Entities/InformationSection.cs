using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class InformationSection
{
    public uint IsId { get; set; }

    public string IsName { get; set; } = null!;

    public uint? IsParentSectionId { get; set; }

    public string IsText { get; set; } = null!;

    public virtual ICollection<InformationSection> InverseIsParentSection { get; set; } = new List<InformationSection>();

    public virtual InformationSection? IsParentSection { get; set; }
}
