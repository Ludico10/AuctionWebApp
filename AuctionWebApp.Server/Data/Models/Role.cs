using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Role
{
    public byte RId { get; set; }

    public string RName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
