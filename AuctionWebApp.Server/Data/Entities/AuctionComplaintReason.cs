using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class AuctionComplaintReason
{
    public ushort AcrId { get; set; }

    public string AcrName { get; set; } = null!;

    public virtual ICollection<AuctionComplaintParameter> AuctionComplaintParameters { get; set; } = new List<AuctionComplaintParameter>();

    public virtual ICollection<AuctionComplaint> AuctionComplaints { get; set; } = new List<AuctionComplaint>();
}
