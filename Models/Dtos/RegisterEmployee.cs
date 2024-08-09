namespace InventoryAPI.Models.Dtos
{
    public class RegisterEmployee
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Phone { get; set; }

    }
}
