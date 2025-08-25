using Ledgerly.API.Models.Domains;
using Ledgerly.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Udemy.Data;

namespace Ledgerly.API.Repositories
{
    public class GlobalParamRepository : IGlobalParamRepository
    {
        private readonly LedgerlyDbContext _db;

        public GlobalParamRepository(LedgerlyDbContext db)
        {
            _db = db;
        }

        public async Task<GlobalParam> CreateGlobalParamAsync(GlobalParam req)
        {
            var newGlobalParam = await _db.GlobalParam.AddAsync(req);
            return newGlobalParam.Entity;
        }

        public async Task<GlobalParam> GetGlobalParamAsync(int id)
        {
            try
            {
                var globalParam = await _db.GlobalParam.FirstOrDefaultAsync(t => t.Id == id);
                return globalParam;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> GetGlobalParamIdByKeyNameAsync(string keyName, string type)
        {
            try
            {
                var globalParam = await _db.GlobalParam.FirstOrDefaultAsync(t => t.KeyName == keyName && t.Type == type);
                var id = globalParam.Id;
                return id;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<GlobalParam>> GetGlobalParamsAsync()
        {
            try
            {
                var globalParams = await _db.GlobalParam.ToListAsync();
                return globalParams;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<GlobalParam> UpdateGlobalParamAsync(GlobalParam req)
        {
            var currentGlobalParam = await _db.GlobalParam.FindAsync(req.Id);
            currentGlobalParam.Name = req.Name;
            currentGlobalParam.KeyName = req.KeyName;
            currentGlobalParam.Type = req.Type;
            currentGlobalParam.Memo = req.Memo;
            return currentGlobalParam;
        }
    }
}
