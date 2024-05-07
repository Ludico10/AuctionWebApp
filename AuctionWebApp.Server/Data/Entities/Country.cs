using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Country
{
    public ushort CouId { get; set; }

    public string CouName { get; set; } = null!;

    public virtual ICollection<CountryDelivery> CountryDeliveries { get; set; } = [];

    public virtual ICollection<User> Users { get; set; } = [];
}
