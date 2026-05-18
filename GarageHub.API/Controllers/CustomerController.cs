using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "customer")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService) 
        => _customerService = customerService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
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

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
        => Ok(await _customerService.GetProfileAsync(GetUserId()));

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest req)
        => Ok(await _customerService.UpdateProfileAsync(GetUserId(), req.FirstName, req.LastName, req.Phone));

    [HttpGet("vehicles")]
    public async Task<IActionResult> GetVehicles()
        => Ok(await _customerService.GetVehiclesAsync(GetUserId()));

    [HttpPost("vehicles")]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleDto vehicleDto)
    {
        try
        {
            var vehicle = await _customerService.AddVehicleAsync(GetUserId(), vehicleDto);
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("purchase-history")]
    public async Task<IActionResult> PurchaseHistory()
        => Ok(await _customerService.GetPurchaseHistoryAsync(GetUserId()));

    [HttpGet("service-history")]
    public async Task<IActionResult> GetServiceHistory()
    {
        try
        {
            var history = await _customerService.GetServiceHistoryAsync(GetUserId());
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record UpdateProfileRequest(string FirstName, string LastName, string Phone);
