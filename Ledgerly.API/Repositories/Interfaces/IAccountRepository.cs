using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAccounts();

        Task<Account> GetAccount(int id);

        Task<Account> CreateAccount(Account req);

        Task<Account> UpdateAccount(Account req);
    }
}
