using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.TransactionType;
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
    public class TransactionTypeService(ITransactionTypeRepository transactionTypeRepository,
        IMapper mapper, 
        LedgerlyDbContext db,
        ICurrentUserService currentUserService) : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository = transactionTypeRepository;
        private readonly IMapper _mapper  = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<ApiResponse<CreateTransactionTypeResponse>> CreateTransactionType(CreateTransactionTypeRequest req)
        {
            // Log Request
            var traceId = Guid.NewGuid();
            _logger.Info($"TransactionTypeService/CreateTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}");

            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"TransactionTypeService/CreateTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<CreateTransactionTypeResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction type
                var transactionType = _mapper.Map<TransactionType>(req);
                transactionType.CreatedBy = userId;
                transactionType.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newTransactionType = await _transactionTypeRepository.CreateTransactionType(transactionType);
                var transactionTypeRes = _mapper.Map<CreateTransactionTypeResponse>(newTransactionType);

                // Add audit log
                await GlobalFunction.RecordAuditLog(userId, "TransactionType", "CreateTransactionType", newTransactionType.Id, newTransactionType.Name, await GetAuditDescription(_db, newTransactionType.Id), _db);

                // Response
                var res = ApiResponse<CreateTransactionTypeResponse>.Success(transactionTypeRes);

                // Log Response
                _logger.Info($"TransactionTypeService/CreateTransactionType, Trace:'{traceId}', Response:{JsonSerializer.Serialize(res)}");

                return res;
            }
            catch(Exception e)
            {
                _logger.Info($"TransactionTypeService/CreateTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateTransactionTypeResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionTypeResponse>> GetTransactionType(GetTransactionTypeRequest req)
        {
            // Log Request
            var traceId = Guid.NewGuid();
            _logger.Info($"TransactionTypeService/GetTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}");

            try
            {
                var transactionType = await _transactionTypeRepository.GetTransactionType(req.id);
                var transactionTypeRes = _mapper.Map<GetTransactionTypeResponse>(transactionType);

                // Response
                var res = ApiResponse<GetTransactionTypeResponse>.Success(transactionTypeRes);

                // Log Response
                _logger.Info($"TransactionTypeService/CreateTransactionType, Trace:'{traceId}', Response:{JsonSerializer.Serialize(res)}");

                return res;
            }
            catch (Exception e)
            {
                _logger.Info($"TransactionTypeService/GetTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetTransactionTypeResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionTypesResponse>> GetTransactionTypes(PaginationRequest req)
        {
            try
            {
                var query = _db.TransactionType.AsQueryable();

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
                    .ProjectTo<TransactionTypesResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Build pagination info
                var pageInfo = new PageInfo(req.Page, req.PageSize, totalRecords);

                return ApiResponse<GetTransactionTypesResponse>.Success(new GetTransactionTypesResponse(data, pageInfo));

            }
            catch (Exception e)
            {
                _logger.Info($"TransactionTypeService/GetTransactionTypes, Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetTransactionTypesResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateTransactionTypeResponse>> UpdateTransactionType(UpdateTransactionTypeRequest req)
        {
            // Log Request
            var traceId = Guid.NewGuid();
            _logger.Info($"TransactionTypeService/UpdateTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}");

            try
            {
                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"TransactionTypeService/UpdateTransactionType, Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<UpdateTransactionTypeResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var transactionType = _mapper.Map<TransactionType>(req);
                transactionType.ModifiedBy = userId;
                transactionType.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentTransactionType = await _transactionTypeRepository.UpdateTransactionType(transactionType);
                var transactionTypeRes = _mapper.Map<UpdateTransactionTypeResponse>(currentTransactionType);

                // Add audit log
                await GlobalFunction.RecordAuditLog(userId, "TransactionType", "UpdateTransactionType", currentTransactionType.Id, currentTransactionType.Name, await GetAuditDescription(_db, currentTransactionType.Id), _db);

                // Response
                var res = ApiResponse<UpdateTransactionTypeResponse>.Success(transactionTypeRes);

                // Log Response
                _logger.Info($"TransactionTypeService/UpdateTransactionType, Trace:'{traceId}', Response:{JsonSerializer.Serialize(res)}");

                return res;
            }
            catch (Exception e)
            {
                _logger.Error($"TransactionTypeService/UpdateTransactionType, Trace:'{traceId}', Request:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<UpdateTransactionTypeResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(LedgerlyDbContext context, int id)
        {
            var brand = await context.TransactionType.Where(x => x.Id == id).Select(x => new
            {
                x.Id,
                x.Name
            }).FirstOrDefaultAsync();
            return JsonSerializer.Serialize(brand);        
        }
    
    }
}
