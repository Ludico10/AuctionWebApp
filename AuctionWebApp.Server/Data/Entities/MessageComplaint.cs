using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class MessageComplaint
{
    public ulong McId { get; set; }

    public byte McTypeId { get; set; }

    public ulong McUserId { get; set; }

    public DateOnly McDate { get; set; }

    public sbyte McConfirmed { get; set; }

    public ulong MsMessageId { get; set; }

    public virtual MessageComplaintType McType { get; set; } = null!;

    public virtual User McUser { get; set; } = null!;
}
