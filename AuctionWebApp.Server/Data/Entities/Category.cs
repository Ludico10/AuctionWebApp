using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Category
{
    public ushort CId { get; set; }

    public string CName { get; set; } = null!;

    public byte СPaidPositionsCount { get; set; }

    public long CPaidPositionCoast { get; set; }

    public virtual ICollection<LotCategory> LotCategories { get; set; } = new List<LotCategory>();
}
