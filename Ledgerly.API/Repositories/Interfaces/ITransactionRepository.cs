using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetTransactionsAsync();

        Task<Transaction> GetTransactionAsync(int id);

        Task<Transaction> CreateTransactionAsync(Transaction req);

        Task<Transaction> UpdateTransactionAsync(Transaction req);

        Task<Transaction> DeleteTransactionAsync(Transaction req);
    }
}
