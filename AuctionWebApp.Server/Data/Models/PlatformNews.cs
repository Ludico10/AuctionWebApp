using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class PlatformNews
{
    public uint PnId { get; set; }

    public string PnTitle { get; set; } = null!;

    public DateTime PnPublicationTime { get; set; }

    public string PnText { get; set; } = null!;
}
