using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class AuctionType
{
    public byte AtId { get; set; }

    public string AtName { get; set; } = null!;

    public virtual ICollection<FinishedAuction> FinishedAuctions { get; set; } = new List<FinishedAuction>();

    public virtual ICollection<Lot> Lots { get; set; } = new List<Lot>();
}
