namespace GarageHub.Application.DTOs
{
    public class TopCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }
    }
}