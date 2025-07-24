using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Text.Json;
using Udemy.Data;

namespace Ledgerly.API.Services
{
    public class AuthService(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        LedgerlyAuthDbContext db,
        IConfiguration configuration) : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ITokenService _tokenService = tokenService;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly LedgerlyAuthDbContext _db = db;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ApiResponse<RegisterResponse>> Register(RegisterRequest req)
        {
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = req.Username,
                    Email = req.Username
                };

                var identityResult = await _userManager.CreateAsync(identityUser, req.Password);

                if (!identityResult.Succeeded)
                {
                    return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.InternalError);
                }

                // Add roles to this user
                var userRole = _configuration["UserSetting:DefaultUserRole"]!;
                if (!await _roleManager.RoleExistsAsync(userRole))
                {
                    await _userManager.DeleteAsync(identityUser);
                    return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.RoleDoesNotExist);
                }

                identityResult = await _userManager.AddToRolesAsync(identityUser, [userRole]);

                return ApiResponse<RegisterResponse>.Success(new RegisterResponse());
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
