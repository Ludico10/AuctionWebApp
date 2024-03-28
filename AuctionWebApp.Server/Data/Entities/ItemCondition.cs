using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class ItemCondition
{
    public byte IcId { get; set; }

    public string IcName { get; set; } = null!;

    public virtual ICollection<FinishedAuction> FinishedAuctions { get; set; } = [];

    public virtual ICollection<Lot> Lots { get; set; } = [];
}
