using System;
using System.Collections.Generic;
using System.Text;

namespace GarageHub.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "customer"; // admin | staff | customer
        public float TotalSpent { get; set; } = 0;
        public float CreditBalance { get; set; } = 0;
        public DateTime? CreditDueDate { get; set; }
        public int? ManagedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Vehicle> Vehicles { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = [];
        public ICollection<PartRequest> PartRequests { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<Notification> Notifications { get; set; } = [];
    }
    Entities/Vehicle.cs
    csharpnamespace GarageHub.Domain.Entities;

    public class Vehicle
    {
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public string VehicleNumber { get; set; } = string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Vin { get; set; } = string.Empty;

        public User User { get; set; } = null!;
        public ICollection<Appointment> Appointments { get; set; } = [];
    }
}
