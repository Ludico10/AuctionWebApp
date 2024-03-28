using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class UserReaction
{
    public ulong UrUserId { get; set; }

    public ulong UrAnswerId { get; set; }

    public byte UrIsPositive { get; set; }

    public virtual Answer UrAnswer { get; set; } = null!;

    public virtual User UrUser { get; set; } = null!;
}
