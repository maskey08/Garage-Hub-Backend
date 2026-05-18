namespace GarageHub.Application.DTOs.PurchaseInvoice
{
    public class PurchaseInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
    }

    public class PurchaseInvoiceItemDto
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public string? PartName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CreatePurchaseInvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<CreatePurchaseInvoiceItemDto> Items { get; set; } = new();
    }

    public class CreatePurchaseInvoiceItemDto
    {
        public int PartId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}