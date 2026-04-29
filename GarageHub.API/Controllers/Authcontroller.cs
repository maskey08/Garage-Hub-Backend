using GarageHub.Application.DTOs.Auth;
using GarageHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GarageHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try { return Ok(await _auth.RegisterAsync(dto)); }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try { return Ok(await _auth.LoginAsync(dto)); }
        catch (Exception ex) { return Unauthorized(new { ex.Message }); }
    }
}