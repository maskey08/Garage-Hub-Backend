namespace GarageHub.Domain.Entities;

public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string PartNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int? VendorId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}