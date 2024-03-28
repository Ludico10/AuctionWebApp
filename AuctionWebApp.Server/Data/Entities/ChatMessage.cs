using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class ChatMessage
{
    public ulong CmId { get; set; }

    public byte CmToSeller { get; set; }

    public string CmText { get; set; } = null!;

    public ulong CmChatId { get; set; }

    public DateTime CmTime { get; set; }

    public virtual Chat CmChat { get; set; } = null!;
}
