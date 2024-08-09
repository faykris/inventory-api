using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryAPI.Models.Entities
{
    [Table("Employee", Schema = "dbo")]
    public class Employee
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; }
        [DefaultValue(2)]
        public int Role { get; set; }
        [DefaultValue(1)]
        public int Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
