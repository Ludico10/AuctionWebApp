using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class FinishedAuction
{
    public ulong FaId { get; set; }

    public string FaName { get; set; } = null!;

    public DateTime FaFinishTime { get; set; }

    public ulong FaSellerId { get; set; }

    public ulong FaWinnerId { get; set; }

    public byte FaAuctionType { get; set; }

    public ulong FaSize { get; set; }

    public string? FaDeliveryAddress { get; set; }

    public string? FaTracingCode { get; set; }

    public byte FaConditionId { get; set; }

    public byte FaCurrentStateId { get; set; }

    public DateTime FaStateUpdateTime { get; set; }

    public string FaDescription { get; set; } = null!;

    public virtual ICollection<AuctionComplaint> AuctionComplaints { get; set; } = new List<AuctionComplaint>();

    public virtual AuctionType FaAuctionTypeNavigation { get; set; } = null!;

    public virtual ItemCondition FaCondition { get; set; } = null!;

    public virtual State FaCurrentState { get; set; } = null!;

    public virtual User FaSeller { get; set; } = null!;

    public virtual User FaWinner { get; set; } = null!;
}
