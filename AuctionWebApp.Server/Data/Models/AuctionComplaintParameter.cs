using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class AuctionComplaintParameter
{
    public ushort AcpComplaintId { get; set; }

    public string AcpName { get; set; } = null!;

    public string AcpExpression { get; set; } = null!;

    public virtual AuctionComplaintReason AcpComplaint { get; set; } = null!;
}
