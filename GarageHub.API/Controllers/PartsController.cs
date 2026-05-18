using GarageHub.Application.DTOs;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class PartsController : ControllerBase
{
    private readonly IPartService _partService;

    public PartsController(IPartService partService)
    {
        _partService = partService;
    }

    [HttpGet]
    public async Task<IActionResult> GetParts([FromQuery] string? search, [FromQuery] string? category)
        => Ok(await _partService.GetPartsAsync(search, category));

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
        => Ok(await _partService.GetLowStockPartsAsync());

    [HttpPost]
    public async Task<IActionResult> AddPart([FromBody] CreatePartDto dto)
    {
        try
        {
            return Ok(await _partService.AddPartAsync(dto));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePart(int id, [FromBody] UpdatePartDto dto)
    {
        try
        {
            return Ok(await _partService.UpdatePartAsync(id, dto));
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePart(int id)
    {
        var removed = await _partService.DeletePartAsync(id);
        return removed ? Ok() : NotFound();
    }
}
