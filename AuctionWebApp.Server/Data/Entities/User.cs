using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionWebApp.Server.Data.Entities
{
    [Table("user")]
    public class User
    {
        [Column("u_id")]
        public int Id { get; set; }

        [Column("u_name")]
        public required string Name { get; set; }

        [Column("u_rating")]
        public byte Rating { get; set; }

        [ForeignKey(nameof(Role))]
        [Column("u_role_id")]
        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }

        [ForeignKey(nameof(Country))]
        [Column("u_country_id")]
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        [Column("u_address")]
        public required string Address { get; set; }

        [Column("u_email")]
        public required string Email { get; set; }

        [Column("u_password_hash")]
        public required string PasswordHash { get; set; }

        [Column("u_balance")]
        public long Balance { get; set; }

        [Column("u_registration_date")]
        public DateTime RegistrationDate { get; set; }
    }
}
