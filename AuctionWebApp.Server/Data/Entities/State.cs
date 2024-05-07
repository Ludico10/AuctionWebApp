using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class State
{
    public byte SId { get; set; }

    public string SName { get; set; } = null!;

    public virtual ICollection<FinishedAuction> FinishedAuctions { get; set; } = new List<FinishedAuction>();
}
