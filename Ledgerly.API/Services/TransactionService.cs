using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.Transaction;
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
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task<ApiResponse<CreateTransactionResponse>> CreateTransaction(CreateTransactionRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"TransactionService/CreateTransaction, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<CreateTransactionResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction 
                var transaction = _mapper.Map<Transaction>(req);
                transaction.UserId = userId;
                transaction.CreatedBy = userId;
                transaction.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newTransaction = await _transactionRepository.CreateTransaction(transaction);
                var transactionRes = _mapper.Map<CreateTransactionResponse>(newTransaction);

                // Add audit log
                var transactionAuditLog = new RecordAuditLog
                {
                    ControllerName = "Transaction",
                    MethodName = "CreateTransaction",
                    TransactionId = newTransaction.Id,
                    TransactionNo = newTransaction.TransactionNo,
                    Description = await GetAuditDescription(newTransaction.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(transactionAuditLog);

                // Response
                return ApiResponse<CreateTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                _logger.Info($"TransactionService/CreateTransaction, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionResponse>> GetTransaction(GetTransactionRequest req)
        {
            try
            {
                var transaction = await _transactionRepository.GetTransaction(req.id);
                var transactionRes = _mapper.Map<GetTransactionResponse>(transaction);

                // Response
                return ApiResponse<GetTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                _logger.Info($"TransactionService/GetTransaction, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionsResponse>> GetTransactions(PaginationRequest req)
        {
            try
            {
                var query = _db.Transaction.AsQueryable();

                var filter = req.Filter;

                //if (filter.Status.HasValue && filter.Status.Value != 0)
                //{
                //    query = query.Where(u => u.StatusId == filter.Status);
                //}

                if (!string.IsNullOrWhiteSpace(filter.Search))
                {
                    var keyword = filter.Search.ToLower();
                    query = query.Where(u =>
                        u.TransactionNo.ToLower().Contains(keyword));
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
                _logger.Error($"TransactionService/GetTransactions, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetTransactionsResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateTransactionResponse>> UpdateTransaction(UpdateTransactionRequest req)
        {
            try
            {
                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"TransactionService/UpdateTransaction, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction 
                var transaction = _mapper.Map<Transaction>(req);
                transaction.UserId = userId;
                transaction.ModifiedBy = userId;
                transaction.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentTransaction = await _transactionRepository.UpdateTransaction(transaction);
                var transactionRes = _mapper.Map<UpdateTransactionResponse>(currentTransaction);

                // Add audit log
                var transactionAuditLog = new RecordAuditLog
                {
                    ControllerName = "Transaction",
                    MethodName = "CreateTransaction",
                    TransactionId = currentTransaction.Id,
                    TransactionNo = currentTransaction.TransactionNo,
                    Description = await GetAuditDescription(currentTransaction.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(transactionAuditLog);

                // Response
                return ApiResponse<UpdateTransactionResponse>.Success(transactionRes);
            }
            catch (Exception e)
            {
                _logger.Error($"TransactionService/UpdateTransaction, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<UpdateTransactionResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(int id)
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            var recordAuditLogTransaction = _mapper.Map<RecordAuditLogTransaction>(transaction);
            return JsonSerializer.Serialize(recordAuditLogTransaction);
        }

    }
}
