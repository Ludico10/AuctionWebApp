using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuctionWebApp.Server.Services
{
    public class UserService(MySqlContext context, ITokenService tokenService) : IUserService
    {
        public async Task<UserShortInfo?> Login(LoginInfo info)
        {
            var user = await context.Users.FirstOrDefaultAsync(u =>
                            (u.UEmail == info.Email) && (u.UPasswordHash == info.Password));
            if (user is null)
            {
                return null;
            }

            var role = await context.Roles.FirstOrDefaultAsync(r => r.RId == user.URoleId);
            if (role is null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, role.RName),
                new(ClaimTypes.Name, info.Email),
                new(ClaimTypes.NameIdentifier, user.UId.ToString())
            };
            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            user.URefreshToken = refreshToken;
            user.URefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await context.SaveChangesAsync();
            var tokens = new TokenApiModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return new UserShortInfo(user, tokens);
        }

        public async Task<bool> Registration(RegistrationInfo registrationInfo)
        {
            try
            {
                var user = new User(registrationInfo);
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetRoleName(byte roleId)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.RId == roleId);
            return role?.RName;
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
