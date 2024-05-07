namespace AuctionWebApp.Server.Data.Dto
{
    public class RegistrationInfo
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
    }
}
