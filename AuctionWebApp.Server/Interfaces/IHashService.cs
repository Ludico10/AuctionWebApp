namespace AuctionWebApp.Server.Interfaces
{
    public interface IHashService
    {
        public string GenerateHash(string password);
        public bool VerifyHash(string password, string hashedValue);
    }
}
