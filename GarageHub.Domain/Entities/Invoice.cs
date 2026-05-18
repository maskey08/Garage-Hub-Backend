using System;

namespace GarageHub.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public int SaleId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = "Generated";

        public Sale Sale { get; set; }
    }
}