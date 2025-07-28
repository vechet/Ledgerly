using Ledgerly.API.Models.Domains;
using Ledgerly.Models;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IStatusRepository
    {
        Task<List<Status>> GetStatuses();

        Task<Status> GetStatus(int id);

        Task<Status> CreateStatus(Status req);

        Task<Status> UpdateStatus(Status req);
    }
}
