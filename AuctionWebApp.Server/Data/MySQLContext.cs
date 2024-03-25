using AuctionWebApp.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionWebApp.Server.Data
{
    public class MySQLContext : DbContext
    {
        public readonly string dbPath = "server=localhost;database=mydb;user=root;password=Phabletik1044";

        public MySQLContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySQL(dbPath)
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Role>()
                .HasData(
                [
                    new() { Id = 1, Name = "Admin" }
                ]);

            modelBuilder
                .Entity<Country>()
                .HasData(
                [
                    new() { Id = 1, Name = "Belarus" }
                ]);

            modelBuilder
                .Entity<User>()
                .HasData(
                [
                    new()
                    {
                        Id = 1,
                        Name = "",
                        Rating = 10,
                        RoleId = 1,
                        CountryId = 1,
                        Address = "",
                        Email = "email@mail.com",
                        PasswordHash = "",
                        Balance = 0,
                        RegistrationDate = DateTime.Now
                    }
                ]);
        }
    }
}
