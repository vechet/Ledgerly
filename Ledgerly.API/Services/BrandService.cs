using AutoMapper;
using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Repositories;
using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Udemy.Data;

namespace Ledgerly.API.Services
{
    public class BrandService(IBrandRepository brandRepository,
        IAccountRepository AccountRepository,
        IMapper mapper,
        LedgerlyDbContext db,
        IUserService currentUserService,
        IAuditLogService auditLogService,
        IGlobalParamRepository globalParamRepository) : IBrandService
    {
        private readonly IBrandRepository _brandRepository = brandRepository;
        private readonly IAccountRepository _AccountRepository = AccountRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private readonly IUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;
        private readonly IGlobalParamRepository _globalParamRepository = globalParamRepository;
        public async Task<ApiResponse<CreateAccountResponse>> CreateAccountAsync(CreateAccountRequest req)
        {
            await using var transaction = await _brandRepository.BeginTransactionAsync();
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("Accountservice", "CreateAccount", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<CreateAccountResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction type
                var account = _mapper.Map<Account>(req);
                account.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                account.UserId = userId;
                account.CreatedBy = userId;
                account.CreatedDate = GlobalFunction.GetCurrentDateTime();
                _brandRepository.Create(account);
                await _brandRepository.SaveChangesAsync();

                await transaction.CommitAsync();
                return null;
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
