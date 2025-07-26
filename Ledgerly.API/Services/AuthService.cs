using AutoMapper;
using Ledgerly.API.Models.DTOs.TransactionType;
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
        ITokenService tokenService,
        LedgerlyAuthDbContext db,
        IConfiguration configuration,
        IMapper mapper) : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;
        private readonly ITokenService _tokenService = tokenService;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly LedgerlyAuthDbContext _db = db;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<RegisterResponse>> Register(RegisterRequest req)
        {
            // Mapping entity
            var identityUser = _mapper.Map<IdentityUser>(req);

            try
            {
                // Add new user
                var identityResult = await _userManager.CreateAsync(identityUser, req.Password);
                if (!identityResult.Succeeded)
                {
                    return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.InternalError);
                }

                // Add roles to new user
                var defaultUserRole = _configuration["UserSetting:DefaultUserRole"];
                identityResult = await _userManager.AddToRolesAsync(identityUser, [defaultUserRole]);

                // Mapping dto
                var res = _mapper.Map<RegisterResponse>(identityUser);

                //response
                return ApiResponse<RegisterResponse>.Success(res);
            }
            catch (Exception e)
            {
                // Remove new user or Rollback
                await _userManager.DeleteAsync(identityUser);

                // Log Info
                _logger.Info($"AuthService/Register, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<RegisterResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<LoginResponse>> Login(LoginRequest req)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(req.Username);
                if (user == null)
                {
                    _logger.Info($"AuthService/LoginResponse, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.UserNotExist}', ErrorMessage:'{ApiResponseStatus.UserNotExist.Description()}'");
                    return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.UserNotExist);
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);
                if (!result.Succeeded)
                {
                    _logger.Info($"AuthService/LoginResponse, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.WrongPassword}', ErrorMessage:'{ApiResponseStatus.WrongPassword.Description()}'");
                    return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.WrongPassword);
                }

                // Generate Access Token
                var roles = await _userManager.GetRolesAsync(user);
                var jwtToken = _tokenService.CreateAccessToken(user, roles.ToList());

                var res = new LoginResponse
                {
                    AccessToken = jwtToken.AccessToken,
                    ExpiresIn = jwtToken.ExpiresIn
                };

                // Response
                return ApiResponse<LoginResponse>.Success(res);
            }
            catch (Exception e)
            {
                _logger.Info($"AuthService/Login, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<LoginResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
