namespace GarageHub.Domain.Entities;

public class SalesInvoiceItem
{
    public int ItemId { get; set; }
    public int SaleId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public float UnitPrice { get; set; }
    public float TotalPrice { get; set; }

    public Part Part { get; set; } = null!;

    public SalesInvoice SalesInvoice { get; set; } = null!;
}