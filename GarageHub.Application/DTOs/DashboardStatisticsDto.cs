namespace GarageHub.Application.DTOs;

public class DashboardStatisticsDto
{
    public int TotalCustomers { get; set; }
    public int TotalStaff { get; set; }
    public int TotalAppointments { get; set; }
    public int PendingAppointments { get; set; }
    public int CompletedAppointments { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
    public int TotalPartRequests { get; set; }
    public int PendingPartRequests { get; set; }
}
