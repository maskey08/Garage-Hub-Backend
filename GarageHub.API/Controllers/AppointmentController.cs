using GarageHub.Application.DTOs.Appointment;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication but don't restrict by role at class level
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> Book(AppointmentCreateDto dto)
    {
        try
        {
            var result = await _appointmentService.BookAsync(GetUserId(), dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> GetMine()
    {
        try
        {
            var appointments = await _appointmentService.GetByCustomerAsync(GetUserId());
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/cancel")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _appointmentService.CancelAsync(id, GetUserId());
        return result ? Ok("Cancelled.") : NotFound("Appointment not found.");
    }
}
