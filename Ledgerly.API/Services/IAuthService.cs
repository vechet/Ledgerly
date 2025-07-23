using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Models.DTOs.User;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResponse>> Register(RegisterRequest req);
        Task<ApiResponse<LoginResponse>> Login(LoginRequest req);

    }
}
