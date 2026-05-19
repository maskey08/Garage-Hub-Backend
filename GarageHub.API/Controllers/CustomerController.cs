using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GarageHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require authentication but don't restrict by role
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly AppDbContext _db;

    public CustomerController(ICustomerService customerService, AppDbContext db)
    {
        _customerService = customerService;
        _db = db;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

    [HttpGet("dashboard")]
    [Authorize(Roles = "customer")]
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
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> GetProfile()
        => Ok(await _customerService.GetProfileAsync(GetUserId()));

    [HttpPut("profile")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest req)
        => Ok(await _customerService.UpdateProfileAsync(GetUserId(), req.FirstName, req.LastName, req.Phone));

    [HttpGet("vehicles")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> GetVehicles()
        => Ok(await _customerService.GetVehiclesAsync(GetUserId()));

    [HttpPost("vehicles")]
    [Authorize(Roles = "customer")]
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

    [HttpPut("vehicles/{vehicleId:int}")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> UpdateVehicle(int vehicleId, [FromBody] AddVehicleDto vehicleDto)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v =>
            v.VehicleId == vehicleId && v.UserId == GetUserId());

        if (vehicle == null)
            return NotFound(new { message = "Vehicle not found" });

        vehicle.VehicleNumber = vehicleDto.VehicleNumber;
        vehicle.Make = vehicleDto.Make;
        vehicle.Model = vehicleDto.Model;
        vehicle.Year = vehicleDto.Year;
        vehicle.Vin = vehicleDto.Vin ?? string.Empty;
        await _db.SaveChangesAsync();

        return Ok(new VehicleResponseDto
        {
            VehicleId = vehicle.VehicleId,
            VehicleNumber = vehicle.VehicleNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Vin = vehicle.Vin
        });
    }

    [HttpDelete("vehicles/{vehicleId:int}")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> DeleteVehicle(int vehicleId)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v =>
            v.VehicleId == vehicleId && v.UserId == GetUserId());

        if (vehicle == null)
            return NotFound(new { message = "Vehicle not found" });

        _db.Vehicles.Remove(vehicle);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("purchase-history")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> PurchaseHistory()
        => Ok(await _customerService.GetCustomerPurchaseHistoryAsync(GetUserId()));

    [HttpGet("service-history")]
    [Authorize(Roles = "customer")]
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
