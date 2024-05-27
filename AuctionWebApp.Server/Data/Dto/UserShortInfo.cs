using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class UserShortInfo
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public byte RoleId { get; set; }
        public int Rating { get; set; }
        public TokenApiModel Tokens { get; set; }

        public UserShortInfo(User user, TokenApiModel tokens)
        {
            Id = user.UId;
            Name = user.UName;
            RoleId = user.URoleId;
            Rating = user.URating;
            Tokens = tokens;
        }
    }
}
