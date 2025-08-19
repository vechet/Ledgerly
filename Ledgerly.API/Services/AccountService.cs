using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Diagnostics;
using System.Text.Json;
using Udemy.Data;
using Ledgerly.API.Repositories;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Enums;
using System.Security.Principal;
using Ledgerly.API.Models.DTOs.Transaction;

namespace Ledgerly.API.Services
{
    public class AccountService(IAccountRepository AccountRepository,
        IMapper mapper,
        LedgerlyDbContext db,
        IUserService currentUserService,
        IAuditLogService auditLogService,
        IGlobalParamRepository globalParamRepository) : IAccountService
    {
        private readonly IAccountRepository _AccountRepository = AccountRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private readonly IUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;
        private readonly IGlobalParamRepository _globalParamRepository = globalParamRepository;

        public async Task<ApiResponse<CreateAccountResponse>> CreateAccount(CreateAccountRequest req)
        {
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
                account.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                account.UserId = userId;
                account.CreatedBy = userId;
                account.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newAccount = await _AccountRepository.CreateAccount(account);
                var accountRes = _mapper.Map<CreateAccountResponse>(newAccount);

                // Add audit log
                var accountAuditLog = new RecordAuditLog
                {
                    ControllerName = "Account",
                    MethodName = "Create",
                    TransactionId = newAccount.Id,
                    TransactionNo = newAccount.Name,
                    Description = await GetAuditDescription(newAccount.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(accountAuditLog);

                // Response
                return ApiResponse<CreateAccountResponse>.Success(accountRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("Accountservice", "CreateAccount", req, e.HResult, e.Message);
                return ApiResponse<CreateAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetAccountResponse>> GetAccount(GetAccountRequest req)
        {
            try
            {
                // Check if account exists
                var getAccount = await _AccountRepository.GetAccount(req.Id);
                if (getAccount == null)
                {
                    LogHelper.Info("Accountservice", "GetAccount", req, ApiResponseStatus.NotFound);
                    return ApiResponse<GetAccountResponse>.Failure(ApiResponseStatus.NotFound);
                }

                var AccountRes = _mapper.Map<GetAccountResponse>(getAccount);

                // Response
                return ApiResponse<GetAccountResponse>.Success(AccountRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("Accountservice", "GetAccount", req, e.HResult, e.Message);
                return ApiResponse<GetAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetAccountsResponse>> GetAccounts(PaginationRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "GetAccounts", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<GetAccountsResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                var isSystemAdminUser = _currentUserService.IsSystemAdminUser();
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                var query = _db.Account.Where(x => x.StatusId == status && (x.UserId == userId || isSystemAdminUser)).AsQueryable();

                var filter = req.Filter;

                //if (filter.Status.HasValue && filter.Status.Value != 0)
                //{
                //    query = query.Where(u => u.StatusId == filter.Status);
                //}

                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    var keyword = filter.Search.ToLower();
                    query = query.Where(u =>
                        u.Name.ToLower().Contains(keyword));
                    //|| u.Memo.ToLower().Contains(keyword));
                }

                if (filter.Sort != null)
                {
                    foreach (var sortOption in filter.Sort)
                    {
                        if (string.IsNullOrWhiteSpace(sortOption.Property) || string.IsNullOrWhiteSpace(sortOption.Direction))
                        {
                            sortOption.Property = "id";
                            sortOption.Direction = "desc";
                        }
                        var isDescending = sortOption.Direction.Equals("desc", StringComparison.OrdinalIgnoreCase);
                        query = query.OrderByDynamic(sortOption.Property, isDescending);
                    }
                }

                // Count total records for pagination
                var totalRecords = await query.CountAsync();

                var data = await query
                    .Skip((req.Page - 1) * req.PageSize)
                    .Take(req.PageSize)
                    .ProjectTo<AccountsResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Build pagination info
                var pageInfo = new PageInfo(req.Page, req.PageSize, totalRecords);

                // Response
                return ApiResponse<GetAccountsResponse>.Success(new GetAccountsResponse(data, pageInfo));
            }
            catch (Exception e)
            {
                LogHelper.Info("Accountservice", "GetAccounts", req, e.HResult, e.Message);
                return ApiResponse<GetAccountsResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest req)
        {
            try
            {
                // Check if account exists
                var getAccount = await _AccountRepository.GetAccount(req.Id);
                if (getAccount == null)
                {
                    LogHelper.Info("Accountservice", "UpdateAccount", req, ApiResponseStatus.NotFound);
                    return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                if (getAccount.StatusId == status)
                {
                    LogHelper.Info("Accountservice", "UpdateAccount", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("Accountservice", "UpdateAccount", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var account = _mapper.Map<Account>(req);
                account.UserId = userId;
                account.ModifiedBy = userId;
                account.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentAccount = await _AccountRepository.UpdateAccount(account);
                var accountRes = _mapper.Map<UpdateAccountResponse>(currentAccount);

                // Add audit log
                var accountAuditLog = new RecordAuditLog
                {
                    ControllerName = "Account",
                    MethodName = "Update",
                    TransactionId = currentAccount.Id,
                    TransactionNo = currentAccount.Name,
                    Description = await GetAuditDescription(currentAccount.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(accountAuditLog);

                // Response
                return ApiResponse<UpdateAccountResponse>.Success(accountRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("Accountservice", "UpdateAccount", req, e.HResult, e.Message);
                return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(int id)
        {
            var Account = await _AccountRepository.GetAccount(id);
            var recordAuditLogAccount = _mapper.Map<RecordAuditLogAccount>(Account);
            return JsonSerializer.Serialize(recordAuditLogAccount);
        }

        public async Task<ApiResponse<DeleteAccountResponse>> DeleteAccount(DeleteAccountRequest req)
        {
            try
            {
                // Check if account exists
                var getAccount = await _AccountRepository.GetAccount(req.Id);
                if (getAccount == null)
                {
                    LogHelper.Info("Accountservice", "DeleteAccount", req, ApiResponseStatus.NotFound);
                    return ApiResponse<DeleteAccountResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                if (getAccount.StatusId == status)
                {
                    LogHelper.Info("Accountservice", "DeleteAccount", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<DeleteAccountResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("Accountservice", "DeleteAccount", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<DeleteAccountResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var account = _mapper.Map<Account>(req);
                account.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.AccountxxxStatus.ToString());
                account.UserId = userId;
                account.ModifiedBy = userId;
                account.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentAccount = await _AccountRepository.DeleteAccount(account);
                var accountRes = _mapper.Map<DeleteAccountResponse>(currentAccount);

                // Add audit log
                var accountAuditLog = new RecordAuditLog
                {
                    ControllerName = "Account",
                    MethodName = "Delete",
                    TransactionId = currentAccount.Id,
                    TransactionNo = currentAccount.Name,
                    Description = await GetAuditDescription(currentAccount.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(accountAuditLog);

                // Response
                return ApiResponse<DeleteAccountResponse>.Success(accountRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("Accountservice", "DeleteAccount", req, e.HResult, e.Message);
                return ApiResponse<DeleteAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
