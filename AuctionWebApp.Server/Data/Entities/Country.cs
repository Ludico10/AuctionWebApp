using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionWebApp.Server.Data.Entities
{
    [Table("country")]
    public class Country
    {
        [Column("cou_id")]
        public int Id { get; set; }

        [Column("cou_name")]
        public required string Name { get; set; }
    }
}
