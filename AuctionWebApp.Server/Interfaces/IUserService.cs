using AuctionWebApp.Server.Data.Entities;

namespace AuctionWebApp.Server.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserById(ulong id);
    }
}
