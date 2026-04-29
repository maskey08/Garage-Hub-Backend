using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.DTOs.Appointment;
using GarageHub.Domain.Entities;

namespace GarageHub.Application.Interfaces;

public interface IAppointmentService
{
    Task<Appointment> BookAsync(int customerId, AppointmentCreateDto dto);
    Task<IEnumerable<Appointment>> GetByCustomerAsync(int customerId);
    Task<bool> CancelAsync(int appointmentId, int customerId);
}