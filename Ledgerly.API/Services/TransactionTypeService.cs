using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Repositories;
using Ledgerly.Helpers;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Text.Json;
using Udemy.Data;

namespace Ledgerly.API.Services
{
    public class TransactionTypeService(ITransactionTypeRepository transactionTypeRepository,
        IMapper mapper, LedgerlyDbContext db) : ITransactionTypeService
    {
        private readonly ITransactionTypeRepository _transactionTypeRepository = transactionTypeRepository;
        private readonly IMapper _mapper  = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public async Task<ApiResponse<CreateTransactionTypeResponse>> CreateTransactionType(CreateTransactionTypeRequest req)
        {
            try
            {
                var a = 0;
                var b = 0;
                var c = a / b;
                var transactionType = _mapper.Map<TransactionType>(req);
                var newTransactionType = await _transactionTypeRepository.CreateTransactionType(transactionType);
                var transactionTypeRes = _mapper.Map<CreateTransactionTypeResponse>(newTransactionType);
                return ApiResponse<CreateTransactionTypeResponse>.Success(transactionTypeRes);
            }
            catch(Exception e)
            {
                _logger.Info($"TransactionTypeService/CreateTransactionType, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateTransactionTypeResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetTransactionTypeResponse>> GetTransactionType(GetTransactionTypeRequest req)
        {
            var transactionType = await _transactionTypeRepository.GetTransactionType(req.id);
            var transactionTypeRes = _mapper.Map<GetTransactionTypeResponse>(transactionType);
            return ApiResponse<GetTransactionTypeResponse>.Success(transactionTypeRes);
        }

        public async Task<ApiResponse<GetTransactionTypesResponse>> GetTransactionTypes(PaginationRequest req)
        {
            var query = _db.transactionTypes.AsQueryable();

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

        public async Task<ApiResponse<UpdateTransactionTypeResponse>> UpdateTransactionType(UpdateTransactionTypeRequest req)
        {
            var transactionType = _mapper.Map<TransactionType>(req);
            var newTransactionType = await _transactionTypeRepository.UpdateTransactionType(transactionType);
            var transactionTypeRes = _mapper.Map<UpdateTransactionTypeResponse>(newTransactionType);
            return ApiResponse<UpdateTransactionTypeResponse>.Success(transactionTypeRes);
        }
    }
}
