using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities;

public class SalesInvoice
{
    public int SaleId { get; set; }
    public int CustomerId { get; set; }
    public int? StaffId { get; set; }
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public float Subtotal { get; set; }
    public float DiscountApplied { get; set; }
    public float TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "pending"; // paid | pending | credit
    public bool CreditUsed { get; set; }

    public User Customer { get; set; } = null!;
    public ICollection<SalesInvoiceItem> Items { get; set; } = [];
}
