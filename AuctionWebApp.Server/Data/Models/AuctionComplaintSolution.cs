using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class AuctionComplaintSolution
{
    public ulong AcsId { get; set; }

    public ulong AcsComplaintId { get; set; }

    public ushort AcsSolutionId { get; set; }

    public ulong AcsAdminId { get; set; }

    public DateOnly AcsDate { get; set; }

    public virtual User AcsAdmin { get; set; } = null!;

    public virtual AuctionComplaint AcsComplaint { get; set; } = null!;

    public virtual PossibleSolution AcsSolution { get; set; } = null!;

    public virtual ICollection<SolutionParameter> SolutionParameters { get; set; } = new List<SolutionParameter>();
}
