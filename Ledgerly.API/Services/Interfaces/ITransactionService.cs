using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<GetTransactionsResponse>> GetTransactionsAsync(PaginationRequest req);

        Task<ApiResponse<GetTransactionResponse>> GetTransactionAsync(GetTransactionRequest req);

        Task<ApiResponse<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest req);

        Task<ApiResponse<UpdateTransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest req);

        Task<ApiResponse<DeleteTransactionResponse>> DeleteTransactionAsync(DeleteTransactionRequest req);

    }
}
