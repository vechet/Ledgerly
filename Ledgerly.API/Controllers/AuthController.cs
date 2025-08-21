using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("v1/auth/register")]
        public async Task<ApiResponse<RegisterResponse>> RegisterAsync([FromBody] RegisterRequest req)
        {
            return await _authService.RegisterAsync(req);
        }

        [HttpPost("v1/auth/login")]
        public async Task<ApiResponse<LoginResponse>> LoginAsync([FromBody] LoginRequest req)
        {
            return await _authService.LoginAsync(req);
        }
    }
}
