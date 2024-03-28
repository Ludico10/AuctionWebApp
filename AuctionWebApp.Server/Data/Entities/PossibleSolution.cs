namespace AuctionWebApp.Server.Data.Entities;

public partial class PossibleSolution
{
    public ushort PsId { get; set; }

    public string PsName { get; set; } = null!;

    public string PsUserNotificationTemplate { get; set; } = null!;

    public string PsSellerNotificationTemplate { get; set; } = null!;

    public virtual ICollection<AuctionComplaintSolution> AuctionComplaintSolutions { get; set; } = [];
}
