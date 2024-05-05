using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuctionWebApp.Server.Services
{
    public class UserService(MySqlContext context, ITokenService tokenService) : IUserService
    {
        public async Task<TokenApiModel?> Login(LoginInfo info)
        {
            var user = await context.Users.FirstOrDefaultAsync(u =>
                            (u.UEmail == info.Email) && (u.UPasswordHash == info.Password));
            if (user is null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, info.Email),
                new(ClaimTypes.Role, user.URole.RName)
            };
            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.URefreshToken = refreshToken;
            user.URefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await context.SaveChangesAsync();
            return new TokenApiModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenApiModel?> RefreshToken(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel.AccessToken is null)
            {
                return null;
            }

            var principal = tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
            if (principal.Identity is null)
            {
                return null;
            }

            var email = principal.Identity.Name;
            var user = await context.Users.SingleOrDefaultAsync(u => u.UEmail == email);
            if (user is null || user.URefreshToken != tokenApiModel.RefreshToken 
                || user.URefreshTokenExpiryTime is null || user.URefreshTokenExpiryTime <= DateTime.Now)
            {
                return null;
            }

            var newAccessToken = tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenService.GenerateRefreshToken();
            user.URefreshToken = newRefreshToken;
            await context.SaveChangesAsync();
            return new TokenApiModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> RevokeToken(string email)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.UEmail == email);
            if (user == null)
            {
                return false;
            }

            user.URefreshToken = null;
            await context.SaveChangesAsync();
            return true;
        }
    }
}
