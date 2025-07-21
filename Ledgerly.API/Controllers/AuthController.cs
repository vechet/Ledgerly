using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager,
        ITokenService tokenService) : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("v1/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var identityUser = new IdentityUser
            {
                UserName = req.Username,
                Email = req.Username
            };

            var identityResult = await _userManager.CreateAsync(identityUser, req.Password);

            if (identityResult.Succeeded)
            {
                return Ok("User was registerd! Please login.");
            }

            return BadRequest("Something went wrong");
        }

        [HttpPost("v1/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _userManager.FindByEmailAsync(req.Username);

            if (user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, req.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create Token
                        var jwtToken = _tokenService.CreateJwtToken(user, roles.ToList());

                        var response = new LoginResponse
                        {
                            Token = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }
    }
}
