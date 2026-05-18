namespace GarageHub.Domain.Entities;

public class Vehicle
{
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
    public string VehicleNumber { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Vin { get; set; } = string.Empty;

    public Customer? Customer { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = [];
}