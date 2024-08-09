using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace InventoryAPI.Models.Entities
{
    [Table("Product", Schema = "dbo")]
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int Elaboration { get; set; }
        public string? ImageUrl { get; set; }
        [DefaultValue(1)]
        public int Status { get; set; }
        public DateTime? Shipped { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; }
    }
}
