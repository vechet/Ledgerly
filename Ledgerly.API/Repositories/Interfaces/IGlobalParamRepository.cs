using Ledgerly.API.Models.Domains;

namespace Ledgerly.API.Repositories.Interfaces
{
    public interface IGlobalParamRepository
    {
        Task<List<GlobalParam>> GetGlobalParams();

        Task<GlobalParam> GetGlobalParam(int id);

        Task<int> GetGlobalParamIdByKeyName(string keyName, string type);

        Task<GlobalParam> CreateGlobalParam(GlobalParam req);

        Task<GlobalParam> UpdateGlobalParam(GlobalParam req);
    }
}
