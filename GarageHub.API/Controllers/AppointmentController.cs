using GarageHub.Application.DTOs.Appointment;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "customer")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Book(AppointmentCreateDto dto)
        => Ok(await _appointmentService.BookAsync(GetUserId(), dto));

    [HttpGet]
    public async Task<IActionResult> GetMine()
        => Ok(await _appointmentService.GetByCustomerAsync(GetUserId()));

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _appointmentService.CancelAsync(id, GetUserId());
        return result ? Ok("Cancelled.") : NotFound("Appointment not found.");
    }
}