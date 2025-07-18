using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services
{
    public interface ITransactionTypeService
    {
        Task<ApiResponse<GetTransactionTypesResponse>> GetTransactionTypes(PaginationRequest req);

        Task<ApiResponse<GetTransactionTypeResponse>> GetTransactionType(GetTransactionTypeRequest req);

        Task<ApiResponse<CreateTransactionTypeResponse>> CreateTransactionType(CreateTransactionTypeRequest req);

        Task<ApiResponse<UpdateTransactionTypeResponse>> UpdateTransactionType(UpdateTransactionTypeRequest req);
    }
}
