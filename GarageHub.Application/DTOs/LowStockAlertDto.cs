namespace GarageHub.Application.DTOs
{
    public class LowStockAlertDto
    {
        public int PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int Threshold { get; set; } = 10;
        public string Status => CurrentStock < Threshold ? "CRITICAL" : "OK";
    }
}