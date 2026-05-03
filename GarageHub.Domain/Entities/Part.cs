namespace GarageHub.Domain.Entities;

public class Part
{
    public int Id { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    // These will be added via migration below
    public string PartNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int LowStockThreshold { get; set; } = 10;
    public int? VendorId { get; set; }
}