using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Data.Dto
{
    public class CommentInfo
    {
        public ulong UserId { get; set; }
        public string UserName { get; set; } = "";
        public DateTime Time { get; set; }
        public string Text { get; set; } = "";

        public CommentInfo(Comment comment)
        {
            UserId = comment.ComUserId;
            UserName = comment.ComUser.UName;
            Time = comment.ComTime;
            Text = comment.ComText;
        }
    }
}
