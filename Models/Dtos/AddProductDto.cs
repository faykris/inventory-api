namespace InventoryAPI.Models.Dtos
{
    public class AddProductDto
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public required int Elaboration { get; set; }
        public string? ImageUrl { get; set; }
        public required int Quantity { get; set; }
    }
}
