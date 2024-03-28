using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class TrackableLot
{
    public ulong TlUserId { get; set; }

    public ulong TlLotId { get; set; }

    public ulong TlMaxAutomaticBid { get; set; }

    public virtual Lot TlLot { get; set; } = null!;

    public virtual User TlUser { get; set; } = null!;
}
