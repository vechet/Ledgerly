using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IGlobalParamRepository
    {
        Task<List<GlobalParam>> GetGlobalParamsAsync();

        Task<GlobalParam> GetGlobalParamAsync(int id);

        Task<int> GetGlobalParamIdByKeyNameAsync(string keyName, string type);

        Task<GlobalParam> CreateGlobalParamAsync(GlobalParam req);

        Task<GlobalParam> UpdateGlobalParamAsync(GlobalParam req);
    }
}
