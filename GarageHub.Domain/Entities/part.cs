namespace GarageHub.Domain.Entities
{
    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public int? VendorId { get; set; }
        public Vendor? Vendor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}