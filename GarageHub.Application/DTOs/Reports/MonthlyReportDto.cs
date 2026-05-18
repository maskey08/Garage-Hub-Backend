namespace GarageHub.Application.DTOs.Reports;

public class MonthlyReportDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<DailyReportDto> DailyBreakdown { get; set; } = new();
}