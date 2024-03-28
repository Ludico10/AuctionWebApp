using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class CountryDelivery
{
    public ulong CdLotId { get; set; }

    public ushort CdCountryId { get; set; }

    public uint CdSize { get; set; }

    public virtual Country CdCountry { get; set; } = null!;

    public virtual Lot CdLot { get; set; } = null!;
}
