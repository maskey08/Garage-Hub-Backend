using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "customer")]
public class CustomerDashboardController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IAppointmentService _appointmentService;
    private readonly IReviewService _reviewService;
    private readonly IPartRequestService _partRequestService;

    public CustomerDashboardController(
        ICustomerService customerService,
        IAppointmentService appointmentService,
        IReviewService reviewService,
        IPartRequestService partRequestService)
    {
        _customerService = customerService;
        _appointmentService = appointmentService;
        _reviewService = reviewService;
        _partRequestService = partRequestService;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        try
        {
            var dashboard = await _customerService.GetCustomerDashboardAsync(GetUserId());
            return Ok(dashboard);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("appointments")]
    public async Task<IActionResult> GetAppointments()
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

    [HttpGet("reviews")]
    public async Task<IActionResult> GetReviews()
    {
        try
        {
            var reviews = await _reviewService.GetByCustomerAsync(GetUserId());
            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("part-requests")]
    public async Task<IActionResult> GetPartRequests()
    {
        try
        {
            var requests = await _partRequestService.GetByCustomerAsync(GetUserId());
            return Ok(requests);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
