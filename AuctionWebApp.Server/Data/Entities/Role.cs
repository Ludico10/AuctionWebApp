using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionWebApp.Server.Data.Entities
{
    [Table("role")]
    public class Role
    {
        [Column("r_id")]
        public int Id { get; set; }

        [Column("r_name")]
        public required string Name { get; set; }
    }
}
