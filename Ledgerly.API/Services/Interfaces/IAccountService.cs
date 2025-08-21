using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<GetAccountsResponse>> GetAccountsAsync(PaginationRequest req);

        Task<ApiResponse<GetAccountResponse>> GetAccountAsync(GetAccountRequest req);

        Task<ApiResponse<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest req);

        Task<ApiResponse<UpdateAccountResponse>> UpdateAccountAsync(UpdateAccountRequest req);

        Task<ApiResponse<DeleteAccountResponse>> DeleteAccountAsync(DeleteAccountRequest req);

    }
}
