using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Application.DTOs
{
    public class CustomerReportDto
    {
        public int CustomerId { get; set; }

        public string Email { get; set; } = string.Empty;

        public int LoyaltyPoints { get; set; }

        public int TotalInvoices { get; set; }

        public decimal TotalSpent { get; set; }
    }
}
