using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResponse>> RegisterAsync(RegisterRequest req);
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest req);

    }
}
