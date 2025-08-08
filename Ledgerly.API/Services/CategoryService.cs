using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.Category;
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
    public class CategoryService(ICategoryRepository CategoryRepository,
        IMapper mapper, 
        LedgerlyDbContext db,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService) : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository = CategoryRepository;
        private readonly IMapper _mapper  = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;

        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategory(CreateCategoryRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"Categorieservice/CreateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction type
                var Category = _mapper.Map<Category>(req);
                Category.UserId = userId;
                Category.CreatedBy = userId;
                Category.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newCategory = await _CategoryRepository.CreateCategory(Category);
                var CategoryRes = _mapper.Map<CreateCategoryResponse>(newCategory);

                // Add audit log
                var CategoryAuditLog = new RecordAuditLog
                {
                    ControllerName = "Category",
                    MethodName = "CreateCategory",
                    TransactionId = newCategory.Id,
                    TransactionNo = newCategory.Name,
                    Description = await GetAuditDescription(newCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(CategoryAuditLog);

                // Response
                return ApiResponse<CreateCategoryResponse>.Success(CategoryRes);
            }
            catch(Exception e)
            {
                _logger.Info($"Categorieservice/CreateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoryResponse>> GetCategory(GetCategoryRequest req)
        {
            try
            {
                var Category = await _CategoryRepository.GetCategory(req.id);
                var CategoryRes = _mapper.Map<GetCategoryResponse>(Category);

                // Response
                return ApiResponse<GetCategoryResponse>.Success(CategoryRes);
            }
            catch (Exception e)
            {
                _logger.Info($"Categorieservice/GetCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoriesResponse>> GetCategories(PaginationRequest req)
        {
            try
            {
                var query = _db.Category.AsQueryable();

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
                    .ProjectTo<CategoriesResponse>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Build pagination info
                var pageInfo = new PageInfo(req.Page, req.PageSize, totalRecords);

                // Response
                return ApiResponse<GetCategoriesResponse>.Success(new GetCategoriesResponse(data, pageInfo));
            }
            catch (Exception e)
            {
                _logger.Error($"Categorieservice/GetCategories, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetCategoriesResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategory(UpdateCategoryRequest req)
        {
            try
            {
                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"Categorieservice/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var Category = _mapper.Map<Category>(req);
                Category.UserId = userId;
                Category.ModifiedBy = userId;
                Category.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentCategory = await _CategoryRepository.UpdateCategory(Category);
                var CategoryRes = _mapper.Map<UpdateCategoryResponse>(currentCategory);

                // Add audit log
                var CategoryAuditLog = new RecordAuditLog
                {
                    ControllerName = "Category",
                    MethodName = "CreateCategory",
                    TransactionId = currentCategory.Id,
                    TransactionNo = currentCategory.Name,
                    Description = await GetAuditDescription(currentCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(CategoryAuditLog);

                // Response
                return ApiResponse<UpdateCategoryResponse>.Success(CategoryRes);
            }
            catch (Exception e)
            {
                _logger.Error($"Categorieservice/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(int id)
        {
            var Category = await _CategoryRepository.GetCategory(id);
            var recordAuditLogCategory = _mapper.Map<RecordAuditLogCategory>(Category);
            return JsonSerializer.Serialize(recordAuditLogCategory);        
        }
    
    }
}
