using System;
using System.Collections.Generic;

namespace AuctionWebApp.Server.Data.Entities;

public partial class User
{
    public ulong UId { get; set; }

    public string UName { get; set; } = null!;

    public byte URating { get; set; }

    public byte URoleId { get; set; }

    public ushort UCountryId { get; set; }

    public string UAddress { get; set; } = null!;

    public string UEmail { get; set; } = null!;

    public string UPasswordHash { get; set; } = null!;

    public long UBalance { get; set; }

    public DateOnly URegistrationDate { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<AuctionComplaintSolution> AuctionComplaintSolutions { get; set; } = new List<AuctionComplaintSolution>();

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();

    public virtual ICollection<Chat> ChatChSellers { get; set; } = new List<Chat>();

    public virtual ICollection<Chat> ChatChUsers { get; set; } = new List<Chat>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<FinishedAuction> FinishedAuctionFaSellers { get; set; } = new List<FinishedAuction>();

    public virtual ICollection<FinishedAuction> FinishedAuctionFaWinners { get; set; } = new List<FinishedAuction>();

    public virtual ICollection<Lot> Lots { get; set; } = new List<Lot>();

    public virtual ICollection<MessageComplaint> MessageComplaints { get; set; } = new List<MessageComplaint>();

    public virtual ICollection<QuestionToAdministration> QuestionToAdministrations { get; set; } = new List<QuestionToAdministration>();

    public virtual ICollection<TrackableLot> TrackableLots { get; set; } = new List<TrackableLot>();

    public virtual Country UCountry { get; set; } = null!;

    public virtual Role URole { get; set; } = null!;

    public virtual ICollection<UserReaction> UserReactions { get; set; } = new List<UserReaction>();
}
