using GarageHub.Application.DTOs.PartRequest;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "customer")]
public class PartRequestController : ControllerBase
{
    private readonly IPartRequestService _partRequestService;
    public PartRequestController(IPartRequestService partRequestService) => _partRequestService = partRequestService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Submit(PartRequestCreateDto dto)
        => Ok(await _partRequestService.SubmitAsync(GetUserId(), dto));

    [HttpGet]
    public async Task<IActionResult> GetMine()
        => Ok(await _partRequestService.GetByCustomerAsync(GetUserId()));
}