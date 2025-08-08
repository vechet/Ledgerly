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

namespace Ledgerly.API.Services
{
    public class AccountService(IAccountRepository AccountRepository,
        IMapper mapper,
        LedgerlyDbContext db,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService) : IAccountService
    {
        private readonly IAccountRepository _AccountRepository = AccountRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task<ApiResponse<CreateAccountResponse>> CreateAccount(CreateAccountRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"Accountservice/CreateAccount, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<CreateAccountResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction type
                var Account = _mapper.Map<Account>(req);
                Account.UserId = userId;
                Account.CreatedBy = userId;
                Account.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newAccount = await _AccountRepository.CreateAccount(Account);
                var AccountRes = _mapper.Map<CreateAccountResponse>(newAccount);

                // Add audit log
                var AccountAuditLog = new RecordAuditLog
                {
                    ControllerName = "Account",
                    MethodName = "CreateAccount",
                    TransactionId = newAccount.Id,
                    TransactionNo = newAccount.Name,
                    Description = await GetAuditDescription(newAccount.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(AccountAuditLog);

                // Response
                return ApiResponse<CreateAccountResponse>.Success(AccountRes);
            }
            catch (Exception e)
            {
                _logger.Info($"Accountservice/CreateAccount, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetAccountResponse>> GetAccount(GetAccountRequest req)
        {
            try
            {
                var Account = await _AccountRepository.GetAccount(req.id);
                var AccountRes = _mapper.Map<GetAccountResponse>(Account);

                // Response
                return ApiResponse<GetAccountResponse>.Success(AccountRes);
            }
            catch (Exception e)
            {
                _logger.Info($"Accountservice/GetAccount, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetAccountsResponse>> GetAccounts(PaginationRequest req)
        {
            try
            {
                var query = _db.Account.AsQueryable();

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
                _logger.Error($"Accountservice/GetAccounts, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetAccountsResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateAccountResponse>> UpdateAccount(UpdateAccountRequest req)
        {
            try
            {
                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"Accountservice/UpdateAccount, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var Account = _mapper.Map<Account>(req);
                Account.UserId = userId;
                Account.ModifiedBy = userId;
                Account.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentAccount = await _AccountRepository.UpdateAccount(Account);
                var AccountRes = _mapper.Map<UpdateAccountResponse>(currentAccount);

                // Add audit log
                var AccountAuditLog = new RecordAuditLog
                {
                    ControllerName = "Account",
                    MethodName = "CreateAccount",
                    TransactionId = currentAccount.Id,
                    TransactionNo = currentAccount.Name,
                    Description = await GetAuditDescription(currentAccount.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(AccountAuditLog);

                // Response
                return ApiResponse<UpdateAccountResponse>.Success(AccountRes);
            }
            catch (Exception e)
            {
                _logger.Error($"Accountservice/UpdateAccount, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<UpdateAccountResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(int id)
        {
            var Account = await _AccountRepository.GetAccount(id);
            var recordAuditLogAccount = _mapper.Map<RecordAuditLogAccount>(Account);
            return JsonSerializer.Serialize(recordAuditLogAccount);
        }

    }
}
