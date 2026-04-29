using System;
using System.Collections.Generic;

namespace GarageHub.Domain.Entities
{
    public class Sale
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public string PaymentMethod { get; set; } = string.Empty;

        public ICollection<SaleItem> SaleItems { get; set; }
            = new List<SaleItem>();
    }
}