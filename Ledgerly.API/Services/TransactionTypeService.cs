using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Repositories;
using Ledgerly.Helpers;

namespace Ledgerly.API.Services
{
    public class TransactionTypeService(ITransactionTypeRepository transactionTypeRepository) : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository = transactionTypeRepository;

        public async Task<ApiResponse<CreateTransactionTypeResponse>> CreateTransactionType(CreateTransactionTypeRequest req)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<GetTransactionTypeResponse>> GetTransactionType(GetTransactionTypeRequest req)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<GetTransactionTypesResponse>> GetTransactionTypes(GetTransactionTypesRequest req)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UpdateTransactionTypeResponse>> UpdateTransactionType(UpdateTransactionTypeRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
