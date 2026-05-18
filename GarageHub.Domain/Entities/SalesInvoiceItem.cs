namespace GarageHub.Domain.Entities;

public class SalesInvoiceItem
{
    public int ItemId { get; set; }
    public int SaleId { get; set; }
    public int PartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public SalesInvoice SalesInvoice { get; set; } = null!;
}