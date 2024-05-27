using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IUserService
    {
        public Task<UserShortInfo?> Login(LoginInfo info);
        public Task<bool> Registration(RegistrationInfo registrationInfo);
        public Task<TokenApiModel?> RefreshToken(TokenApiModel tokenApiModel);
        public Task<bool> RevokeToken(string email);
        public Task<string?> GetRoleName(byte roleId);
    }
}
