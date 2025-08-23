using Ledgerly.API.Models.Domains;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        void Create(Account account);

        Task<IDbContextTransaction> BeginTransactionAsync();

        Task SaveChangesAsync();
    }
}
