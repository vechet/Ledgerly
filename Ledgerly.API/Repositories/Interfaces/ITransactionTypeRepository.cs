using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ITransactionTypeRepository
    {
        Task<List<TransactionType>> GetTransactionTypes();

        Task<TransactionType> GetTransactionType(int id);

        Task<TransactionType> CreateTransactionType(TransactionType req);

        Task<TransactionType> UpdateTransactionType(TransactionType req);
    }
}
