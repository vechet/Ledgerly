using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactions();

        Task<Transaction> GetTransaction(int id);

        Task<Transaction> CreateTransaction(Transaction req);

        Task<Transaction> UpdateTransaction(Transaction req);
    }
}
