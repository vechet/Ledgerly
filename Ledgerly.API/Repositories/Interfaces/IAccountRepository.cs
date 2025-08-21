using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccountsAsync();

        Task<Account> GetAccountAsync(int id);

        Task<Account> CreateAccountAsync(Account req);

        Task<Account> UpdateAccountAsync(Account req);

        Task<Account> DeleteAccountAsync(Account req);
    }
}
