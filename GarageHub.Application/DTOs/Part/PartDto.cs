namespace GarageHub.Application.DTOs.Part
{
    public class PartDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public int? VendorId { get; set; }
        public string? VendorName { get; set; }
    }

    public class CreatePartDto
    {
        public string Name { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public int? VendorId { get; set; }
    }

    public class UpdatePartDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public int? VendorId { get; set; }
    }
}