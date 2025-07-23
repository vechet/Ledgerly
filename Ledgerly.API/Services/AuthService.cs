using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Text.Json;

namespace Ledgerly.API.Services
{
    public class AuthService(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService) : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ITokenService _tokenService = tokenService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<ApiResponse<RegisterResponse>> Register(RegisterRequest req)
        {
            try
            {
                var validRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                var invalidRoles = req.Roles.Except(validRoles).ToList();

                if (invalidRoles.Any())
                {
                    //return BadRequest(new { message = $"Invalid roles: {string.Join(", ", invalidRoles)}" });
                }

                var identityUser = new IdentityUser
                {
                    UserName = req.Username,
                    Email = req.Username
                };

                var identityResult = await _userManager.CreateAsync(identityUser, req.Password);

                if (identityResult.Succeeded)
                {
                    // Add roles to this user
                    if (req.Roles != null && req.Roles.Any())
                    {
                        identityResult = await _userManager.AddToRolesAsync(identityUser, req.Roles);

                        if (identityResult.Succeeded)
                        {
                            return ApiResponse<RegisterResponse>.Success(null);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.Info($"AuthService/Register, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest req)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(req.Username);

                //handle when user doesn't exist


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
                            var jwtToken = _tokenService.CreateAccessToken(user, roles.ToList());

                            var res = new LoginResponse
                            {
                                AccessToken = jwtToken.AccessToken,
                                ExpiresIn = jwtToken.ExpiresIn
                            };
                            return ApiResponse<LoginResponse>.Success(res);
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.Info($"AuthService/Login, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
