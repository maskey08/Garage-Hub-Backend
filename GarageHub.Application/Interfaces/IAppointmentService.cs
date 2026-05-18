using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.DTOs.Appointment;

namespace GarageHub.Application.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> BookAsync(int customerId, AppointmentCreateDto dto);
    Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(int customerId);
    Task<bool> CancelAsync(int appointmentId, int customerId);
}
