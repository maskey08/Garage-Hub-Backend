using GarageHub.Application.Interfaces;
using GarageHub.Domain.Entities;
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
    public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        => Ok(await _customerService.AddVehicleAsync(GetUserId(), vehicle));

    [HttpGet("purchase-history")]
    public async Task<IActionResult> PurchaseHistory()
        => Ok(await _customerService.GetPurchaseHistoryAsync(GetUserId()));
}

public record UpdateProfileRequest(string FirstName, string LastName, string Phone);
