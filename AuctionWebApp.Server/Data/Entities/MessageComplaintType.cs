using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class MessageComplaintType
{
    public byte MctId { get; set; }

    public string MctName { get; set; } = null!;

    public sbyte MctRatingPanishment { get; set; }

    public virtual ICollection<MessageComplaint> MessageComplaints { get; set; } = new List<MessageComplaint>();
}
