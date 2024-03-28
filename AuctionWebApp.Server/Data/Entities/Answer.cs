using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class Answer
{
    public ulong AId { get; set; }

    public ulong AUserId { get; set; }

    public string AText { get; set; } = null!;

    public uint AQuestionId { get; set; }

    public DateTime ATime { get; set; }

    public virtual QuestionToAdministration AQuestion { get; set; } = null!;

    public virtual User AUser { get; set; } = null!;

    public virtual ICollection<UserReaction> UserReactions { get; set; } = [];
}
