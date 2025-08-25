using AutoMapper;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Data;
using System.Text.Json;
using Udemy.Data;

namespace Ledgerly.API.Services
{
    public class AuthService(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IJwtService tokenService,
        LedgerlyAuthDbContext db,
        IConfiguration configuration,
        IMapper mapper) : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly IJwtService _tokenService = tokenService;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly LedgerlyAuthDbContext _db = db;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest req)
        {
            // Mapping entity
            var identityUser = _mapper.Map<IdentityUser>(req);

            try
            {
                // Add new user
                var identityResult = await _userManager.CreateAsync(identityUser, req.Password);
                if (!identityResult.Succeeded)
                {
                    LogHelper.Info("AuthService", "Register", req, ApiResponseStatus.DuplicateUserName);
                    return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.DuplicateUserName);
                }

                // Add roles to new user
                var defaultUserRole = _configuration["UserSetting:DefaultUserRole"];
                identityResult = await _userManager.AddToRolesAsync(identityUser, [defaultUserRole]);

                //response
                var res = _mapper.Map<RegisterResponse>(identityUser);
                return ApiResponse<RegisterResponse>.Success(res);
            }
            catch (Exception e)
            {
                // Remove new user or Rollback
                await _userManager.DeleteAsync(identityUser);

                LogHelper.Info("AuthService", "Register", req, e.HResult, e.Message);
                return ApiResponse<RegisterResponse>.Failure(e.HResult, e.Message);
            }
        }

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest req)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(req.Username);
                if (user == null)
                {
                    LogHelper.Info("AuthService", "Login", req, ApiResponseStatus.UserNotExist);
                    return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.UserNotExist);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
                if (!result.Succeeded)
                {
                    LogHelper.Info("AuthService", "Login", req, ApiResponseStatus.WrongPassword);
                    return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.WrongPassword);
                }

                // Generate Access Token
                var roles = await _userManager.GetRolesAsync(user);
                var jwtToken = _tokenService.GenerateToken(user, roles.ToList());

                // Response
                var res = new LoginResponse
                {
                    AccessToken = jwtToken.AccessToken,
                    ExpiresIn = jwtToken.ExpiresIn
                };
                return ApiResponse<LoginResponse>.Success(res);
            }
            catch (Exception e)
            {
                LogHelper.Info("AuthService", "Login", req, e.HResult, e.Message);
                return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
