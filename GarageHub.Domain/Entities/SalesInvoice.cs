using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class SalesInvoice
{
    public int SaleId { get; set; }
    public int CustomerId { get; set; }
    public int? StaffId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountApplied { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "pending"; // paid | pending | credit
    public bool CreditUsed { get; set; }

    public User Customer { get; set; } = null!;
    public ICollection<SalesInvoiceItem> Items { get; set; } = [];
}
