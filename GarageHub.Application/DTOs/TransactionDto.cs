namespace GarageHub.Application.DTOs;

public class TransactionDto
{
    public int Id { get; set; }
    public string InvoiceId { get; set; } = string.Empty;
    public string Customer { get; set; } = string.Empty;
    public string PartSold { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
    public string Status { get; set; } = "completed";
}
