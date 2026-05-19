using GarageHub.Application.DTOs.PartRequest;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PartRequestController : ControllerBase
{
    private readonly IPartRequestService _partRequestService;
    public PartRequestController(IPartRequestService partRequestService) => _partRequestService = partRequestService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> Submit(PartRequestCreateDto dto)
        => Ok(await _partRequestService.SubmitAsync(GetUserId(), dto));

    [HttpGet]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> GetMine()
        => Ok(await _partRequestService.GetByCustomerAsync(GetUserId()));

    [HttpGet("all")]
    [Authorize(Roles = "staff,admin")]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _partRequestService.GetAllAsync();
        return Ok(requests.Select(request => new
        {
            request.RequestId,
            request.CustomerId,
            request.PartName,
            request.Description,
            request.Status,
            request.RequestedAt,
            Customer = request.Customer == null ? null : new
            {
                request.Customer.FirstName,
                request.Customer.LastName,
                request.Customer.Email,
                request.Customer.Phone
            }
        }));
    }
}
