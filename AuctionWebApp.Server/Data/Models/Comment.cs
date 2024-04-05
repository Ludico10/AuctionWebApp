using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Comment
{
    public ulong ComId { get; set; }

    public ulong ComLotId { get; set; }

    public ulong ComUserId { get; set; }

    public string ComText { get; set; } = null!;

    public DateTime ComTime { get; set; }

    public virtual Lot ComLot { get; set; } = null!;

    public virtual User ComUser { get; set; } = null!;
}
