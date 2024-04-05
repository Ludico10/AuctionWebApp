using AuctionWebApp.Server.Data.Dto;
using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class LotAdditionalParameter
{
    public string LapName { get; set; } = null!;

    public ulong LapLotId { get; set; }

    public string? LapValue { get; set; }

    public virtual Lot LapLot { get; set; } = null!;
}
