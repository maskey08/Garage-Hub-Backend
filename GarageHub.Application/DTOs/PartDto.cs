namespace GarageHub.Application.DTOs;

public class PartDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Status { get; set; } = "in_stock";
    public int LowStockThreshold { get; set; }
}

public class CreatePartDto
{
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int LowStockThreshold { get; set; }
}

public class UpdatePartDto
{
    public string? Name { get; set; }
    public string? Sku { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public int? LowStockThreshold { get; set; }
}

public class LowStockPartDto
{
    public int PartId { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinThreshold { get; set; }
}
