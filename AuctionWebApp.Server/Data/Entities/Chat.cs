using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Chat
{
    public ulong ChId { get; set; }

    public ulong ChSellerId { get; set; }

    public ulong ChUserId { get; set; }

    public byte ChIsBlocked { get; set; }

    public virtual User ChSeller { get; set; } = null!;

    public virtual User ChUser { get; set; } = null!;

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
