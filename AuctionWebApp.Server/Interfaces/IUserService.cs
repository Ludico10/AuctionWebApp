using AuctionWebApp.Server.Data.Dto;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IUserService
    {
        public Task<TokenApiModel?> Login(LoginInfo info);
        public Task<TokenApiModel?> RefreshToken(TokenApiModel tokenApiModel);
        public Task<bool> RevokeToken(string email);
    }
}
