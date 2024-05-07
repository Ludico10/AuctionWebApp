using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class SolutionParameter
{
    public string SpName { get; set; } = null!;

    public ulong SpSolutionId { get; set; }

    public string? SpValue { get; set; }

    public virtual AuctionComplaintSolution SpSolution { get; set; } = null!;
}
