using System;
using System.Collections.Generic;
using System.Text;
using GarageHub.Application.DTOs.Auth;

namespace GarageHub.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}