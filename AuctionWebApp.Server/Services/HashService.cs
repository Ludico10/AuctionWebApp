using AuctionWebApp.Server.Interfaces;

namespace AuctionWebApp.Server.Services
{
    public class HashService : IHashService
    {
        public string GenerateHash(string password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }

        public bool VerifyHash(string password, string hashedValue)
        {
            bool verified = BCrypt.Net.BCrypt.Verify(password, hashedValue);
            return verified;
        }
    }
}
