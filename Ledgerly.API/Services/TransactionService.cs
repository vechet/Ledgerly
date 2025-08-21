using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.API.Repositories;
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
    public class TransactionService(ITransactionRepository transactionRepository,
        IMapper mapper,
        LedgerlyDbContext db,
        IUserService currentUserService,
        IAuditLogService auditLogService,
        IGlobalParamRepository globalParamRepository) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private readonly IUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;
        private readonly IGlobalParamRepository _globalParamRepository = globalParamRepository;

        public async Task<ApiResponse<CreateTransactionResponse>> CreateTransactionAsync(CreateTransactionRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "CreateTransaction", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<CreateTransactionResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction 
                var transaction = _mapper.Map<Transaction>(req);
                transaction.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.TransactionxxxStatus.ToString());
                transaction.UserId = userId;
                transaction.CreatedBy = userId;
                transaction.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newTransaction = await _transactionRepository.CreateTransactionAsync(transaction);
                var transactionRes = _mapper.Map<CreateTransactionResponse>(newTransaction);

                // Add audit log
                var transactionAuditLog = new RecordAuditLog
                {
                    ControllerName = "Transaction",
                    MethodName = "Create",
                    TransactionId = newTransaction.Id,
                    TransactionNo = "",
                    Description = await GetAuditDescriptionAsync(newTransaction.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLogAsync(transactionAuditLog);

                // Response
                return ApiResponse<CreateTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("TransactionService", "CreateTransaction", req, e.HResult, e.Message);
                return ApiResponse<CreateTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionResponse>> GetTransactionAsync(GetTransactionRequest req)
        {
            try
            {
                // Check if account exists
                var getTransaction = await _transactionRepository.GetTransactionAsync(req.Id);
                if (getTransaction == null)
                {
                    LogHelper.Info("TransactionService", "GetTransaction", req, ApiResponseStatus.NotFound);
                    return ApiResponse<GetTransactionResponse>.Failure(ApiResponseStatus.NotFound);
                }

                var transactionRes = _mapper.Map<GetTransactionResponse>(getTransaction);

                // Response
                return ApiResponse<GetTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("TransactionService", "GetTransaction", req, e.HResult, e.Message);
                return ApiResponse<GetTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionsResponse>> GetTransactionsAsync(PaginationRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "GetTransactions", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<GetTransactionsResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                var isSystemAdminUser = _currentUserService.IsSystemAdminUser();
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.TransactionxxxStatus.ToString());
                var query = _db.Transaction.Where(x => x.StatusId == status && (x.UserId == userId || isSystemAdminUser)).AsQueryable();

                var filter = req.Filter;

                //if (filter.Status.HasValue && filter.Status.Value != 0)
                //{
                //    query = query.Where(u => u.StatusId == filter.Status);
                //}

                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    var keyword = filter.Search.ToLower();
                    //query = query.Where(u =>
                    //    u.Name.ToLower().Contains(keyword));
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
                    .ProjectTo<TransactionsResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Build pagination info
                var pageInfo = new PageInfo(req.Page, req.PageSize, totalRecords);

                // Response
                return ApiResponse<GetTransactionsResponse>.Success(new GetTransactionsResponse(data, pageInfo));
            }
            catch (Exception e)
            {
                LogHelper.Info("TransactionService", "GetTransactions", req, e.HResult, e.Message);
                return ApiResponse<GetTransactionsResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateTransactionResponse>> UpdateTransactionAsync(UpdateTransactionRequest req)
        {
            try
            {
                // Check if account exists
                var getTransaction = await _transactionRepository.GetTransactionAsync(req.Id);
                if (getTransaction == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.NotFound);
                    return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.TransactionxxxStatus.ToString());
                if (getTransaction == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction 
                var transaction = _mapper.Map<Transaction>(req);
                transaction.UserId = userId;
                transaction.ModifiedBy = userId;
                transaction.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentTransaction = await _transactionRepository.UpdateTransactionAsync(transaction);
                var transactionRes = _mapper.Map<UpdateTransactionResponse>(currentTransaction);

                // Add audit log
                var transactionAuditLog = new RecordAuditLog
                {
                    ControllerName = "Transaction",
                    MethodName = "Update",
                    TransactionId = currentTransaction.Id,
                    TransactionNo = "",
                    Description = await GetAuditDescriptionAsync(currentTransaction.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLogAsync(transactionAuditLog);

                // Response
                return ApiResponse<UpdateTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("TransactionService", "UpdateTransaction", req, e.HResult, e.Message);
                return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescriptionAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionAsync(id);
            var recordAuditLogTransaction = _mapper.Map<RecordAuditLogTransaction>(transaction);
            return JsonSerializer.Serialize(recordAuditLogTransaction);
        }

        public async Task<ApiResponse<DeleteTransactionResponse>> DeleteTransactionAsync(DeleteTransactionRequest req)
        {
            try
            {
                // Check if account exists
                var getTransaction = await _transactionRepository.GetTransactionAsync(req.Id);
                if (getTransaction == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.NotFound);
                    return ApiResponse<DeleteTransactionResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.TransactionxxxStatus.ToString());
                if (getTransaction == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<DeleteTransactionResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "UpdateTransaction", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<DeleteTransactionResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction 
                var transaction = _mapper.Map<Transaction>(req);
                transaction.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.TransactionxxxStatus.ToString());
                transaction.UserId = userId;
                transaction.ModifiedBy = userId;
                transaction.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentTransaction = await _transactionRepository.DeleteTransactionAsync(transaction);
                var transactionRes = _mapper.Map<DeleteTransactionResponse>(currentTransaction);

                // Add audit log
                var transactionAuditLog = new RecordAuditLog
                {
                    ControllerName = "Transaction",
                    MethodName = "Delete",
                    TransactionId = currentTransaction.Id,
                    TransactionNo = "",
                    Description = await GetAuditDescriptionAsync(currentTransaction.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLogAsync(transactionAuditLog);

                // Response
                return ApiResponse<DeleteTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("TransactionService", "DeleteTransaction", req, e.HResult, e.Message);
                return ApiResponse<DeleteTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
