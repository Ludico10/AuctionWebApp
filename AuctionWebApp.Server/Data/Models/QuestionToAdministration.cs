using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class QuestionToAdministration
{
    public uint QtaId { get; set; }

    public string QtaQuestion { get; set; } = null!;

    public ulong QtaUserId { get; set; }

    public byte QtaSolved { get; set; }

    public DateTime QtaDate { get; set; }

    public int QuaRating { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual User QtaUser { get; set; } = null!;
}
