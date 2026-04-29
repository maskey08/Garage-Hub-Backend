namespace GarageHub.Domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaymentDueDate { get; set; }

        public Customer? Customer { get; set; }
    }
}