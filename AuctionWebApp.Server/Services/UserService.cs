using AuctionWebApp.Server.Data;
using AuctionWebApp.Server.Data.Entities;
using AuctionWebApp.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Services
{
    public class UserService(MySqlContext context) : IUserService
    {
        public async Task<User?> GetUserById(ulong id)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.UId == id);
        }
    }
}
