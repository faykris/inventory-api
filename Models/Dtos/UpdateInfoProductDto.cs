namespace InventoryAPI.Models.Dtos
{
    public class UpdateInfoProductDto
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public required int Elaboration { get; set; }
        public required string ImageUrl { get; set; }
    }
}
