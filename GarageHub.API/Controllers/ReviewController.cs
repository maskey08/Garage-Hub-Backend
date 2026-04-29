using GarageHub.Application.DTOs.Review;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "customer")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    public ReviewController(IReviewService reviewService) => _reviewService = reviewService;

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Submit(ReviewCreateDto dto)
    {
        try { return Ok(await _reviewService.SubmitAsync(GetUserId(), dto)); }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [HttpGet]
    public async Task<IActionResult> GetMine()
        => Ok(await _reviewService.GetByCustomerAsync(GetUserId()));
}