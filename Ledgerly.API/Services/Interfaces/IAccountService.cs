using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApiResponse<GetAccountsResponse>> GetAccounts(PaginationRequest req);

        Task<ApiResponse<GetAccountResponse>> GetAccount(GetAccountRequest req);

        Task<ApiResponse<CreateAccountResponse>> CreateAccount(CreateAccountRequest req);

        Task<ApiResponse<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest req);

        Task<ApiResponse<DeleteAccountResponse>> DeleteAccount(DeleteAccountRequest req);

    }
}
