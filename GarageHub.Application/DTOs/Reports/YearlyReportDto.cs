namespace GarageHub.Application.DTOs.Reports;

public class YearlyReportDto
{
    public int Year { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public List<MonthlyReportDto> MonthlyBreakdown { get; set; } = new();
}