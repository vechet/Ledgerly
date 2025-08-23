using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IBrandService
    {
        Task<ApiResponse<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest req);
    }
}
