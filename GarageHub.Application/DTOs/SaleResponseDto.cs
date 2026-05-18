using System;

namespace GarageHub.Application.DTOs
{
    public class SaleResponseDto
    {
        public int SaleId { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal GrandTotal { get; set; }

        public DateTime SaleDate { get; set; }

        public int PointsUsed { get; set; }

        public int PointsEarned { get; set; }
    }
}