using GarageHub.Application.DTOs.Appointment;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GarageHub.Infrastructure.Data;

namespace GarageHub.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;
    public AppointmentService(AppDbContext db) => _db = db;

    public async Task<AppointmentResponseDto> BookAsync(int customerId, AppointmentCreateDto dto)
    {
        var appointment = new Appointment
        {
            CustomerId = customerId,
            VehicleId = dto.VehicleId,
            ScheduledAt = dto.ScheduledAt,
            ServiceType = dto.ServiceType,
            Notes = dto.Notes,
            Status = "pending"
        };
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync();

        // Get the vehicle info
        var vehicle = await _db.Vehicles.FindAsync(dto.VehicleId);
        var vehicleInfo = vehicle != null ? $"{vehicle.Make} {vehicle.Model} ({vehicle.VehicleNumber})" : "";

        return new AppointmentResponseDto
        {
            AppointmentId = appointment.AppointmentId,
            CustomerId = appointment.CustomerId,
            VehicleId = appointment.VehicleId,
            VehicleInfo = vehicleInfo,
            ScheduledAt = appointment.ScheduledAt,
            ServiceType = appointment.ServiceType,
            Status = appointment.Status,
            Notes = appointment.Notes
        };
    }

    public async Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(int customerId)
    {
        var appointments = await _db.Appointments
            .Include(a => a.Vehicle)
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync();

        return appointments.Select(a => new AppointmentResponseDto
        {
            AppointmentId = a.AppointmentId,
            CustomerId = a.CustomerId,
            VehicleId = a.VehicleId,
            VehicleInfo = a.Vehicle != null ? $"{a.Vehicle.Make} {a.Vehicle.Model} ({a.Vehicle.VehicleNumber})" : "",
            ScheduledAt = a.ScheduledAt,
            ServiceType = a.ServiceType,
            Status = a.Status,
            Notes = a.Notes
        });
    }

    public async Task<bool> CancelAsync(int appointmentId, int customerId)
    {
        var appt = await _db.Appointments
            .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId && a.CustomerId == customerId);
        if (appt is null) return false;
        appt.Status = "cancelled";
        await _db.SaveChangesAsync();
        return true;
    }
}
