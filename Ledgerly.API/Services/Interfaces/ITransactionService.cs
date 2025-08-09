using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<ApiResponse<GetTransactionsResponse>> GetTransactions(PaginationRequest req);

        Task<ApiResponse<GetTransactionResponse>> GetTransaction(GetTransactionRequest req);

        Task<ApiResponse<CreateTransactionResponse>> CreateTransaction(CreateTransactionRequest req);

        Task<ApiResponse<UpdateTransactionResponse>> UpdateTransaction(UpdateTransactionRequest req);

        Task<ApiResponse<DeleteTransactionResponse>> DeleteTransaction(DeleteTransactionRequest req);

    }
}
