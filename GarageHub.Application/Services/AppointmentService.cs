using GarageHub.Application.DTOs.Appointment;
using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using GarageHub.Infrastructure.Data;

namespace GarageHub.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly AppDbContext _db;
    public AppointmentService(AppDbContext db) => _db = db;

    public async Task<Appointment> BookAsync(int customerId, AppointmentCreateDto dto)
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
        return appointment;
    }

    public async Task<IEnumerable<Appointment>> GetByCustomerAsync(int customerId)
        => await _db.Appointments
            .Include(a => a.Vehicle)
            .Where(a => a.CustomerId == customerId)
            .OrderByDescending(a => a.ScheduledAt)
            .ToListAsync();

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