using AuctionWebApp.Server.Data.Dto;
using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class AuctionComplaint
{
    public ulong AcId { get; set; }

    public ushort AcReasonId { get; set; }

    public string AcDescription { get; set; } = null!;

    public ulong AcAuctionId { get; set; }

    public sbyte AcSolved { get; set; }

    public DateOnly AcDate { get; set; }

    public virtual FinishedAuction AcAuction { get; set; } = null!;

    public virtual AuctionComplaintReason AcReason { get; set; } = null!;

    public virtual ICollection<AuctionComplaintSolution> AuctionComplaintSolutions { get; set; } = new List<AuctionComplaintSolution>();

    public AuctionComplaint() { }

    public AuctionComplaint(ComplaintRequest request, DateOnly date)
    {
        AcReasonId = request.ReasonId;
        AcDescription = request.Comment;
        AcAuctionId = request.LotId;
        AcDate = date;
        AcSolved = 0;
    }
}
