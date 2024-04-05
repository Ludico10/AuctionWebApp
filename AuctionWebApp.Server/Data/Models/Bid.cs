using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Bid
{
    public ulong BId1 { get; set; }

    public DateTime BTime { get; set; }

    public ulong BParticipantId { get; set; }

    public ulong BSize { get; set; }

    public ulong BLotId { get; set; }

    public virtual Lot BLot { get; set; } = null!;

    public virtual User BParticipant { get; set; } = null!;
}
