using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.AuditLog;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Repositories;
using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.Json;
using Udemy.Data;

namespace Ledgerly.API.Services
{
    public class CategoryService(ICategoryRepository CategoryRepository,
        IMapper mapper,
        LedgerlyDbContext db,
        IUserService currentUserService,
        IAuditLogService auditLogService,
        IGlobalParamRepository globalParamRepository) : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository = CategoryRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService _currentUserService = currentUserService;
        private readonly IAuditLogService _auditLogService = auditLogService;
        private readonly IGlobalParamRepository _globalParamRepository = globalParamRepository;

        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategory(CreateCategoryRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"CategoryService/CreateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // add new transaction type
                var category = _mapper.Map<Category>(req);
                category.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                category.UserId = userId;
                category.CreatedBy = userId;
                category.CreatedDate = GlobalFunction.GetCurrentDateTime();
                var newCategory = await _CategoryRepository.CreateCategory(category);
                var categoryRes = _mapper.Map<CreateCategoryResponse>(newCategory);

                // Add audit log
                var categoryAuditLog = new RecordAuditLog
                {
                    ControllerName = "Category",
                    MethodName = "CreateCategory",
                    TransactionId = newCategory.Id,
                    TransactionNo = newCategory.Name,
                    Description = await GetAuditDescription(newCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(categoryAuditLog);

                // Response
                return ApiResponse<CreateCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                _logger.Info($"CategoryService/CreateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoryResponse>> GetCategory(GetCategoryRequest req)
        {
            try
            {
                // Check if category exists
                var getCategory = await _CategoryRepository.GetCategory(req.Id);
                if (getCategory == null)
                {
                    _logger.Error($"CategoryService/GetCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.NotFound.Value()}', ErrorMessage:'{ApiResponseStatus.NotFound.Description()}'");
                    return ApiResponse<GetCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                var categoryRes = _mapper.Map<GetCategoryResponse>(getCategory);

                // Response
                return ApiResponse<GetCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                _logger.Info($"CategoryService/GetCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoriesResponse>> GetCategories(PaginationRequest req)
        {
            try
            {
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                var query = _db.Category.Where(x => x.StatusId == status).AsQueryable();

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
                _logger.Error($"CategoryService/GetCategories, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<GetCategoriesResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategory(UpdateCategoryRequest req)
        {
            try
            {
                // Check if category exists
                var getCategory = await _CategoryRepository.GetCategory(req.Id);
                if (getCategory == null)
                {
                    _logger.Error($"CategoryService/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.NotFound.Value()}', ErrorMessage:'{ApiResponseStatus.NotFound.Description()}'");
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                if (getCategory == null)
                {
                    _logger.Error($"CategoryService/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.AlreadyDeleted.Value()}', ErrorMessage:'{ApiResponseStatus.AlreadyDeleted.Description()}'");
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"CategoryService/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update transaction type
                var category = _mapper.Map<Category>(req);
                category.UserId = userId;
                category.ModifiedBy = userId;
                category.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentCategory = await _CategoryRepository.UpdateCategory(category);
                var categoryRes = _mapper.Map<UpdateCategoryResponse>(currentCategory);

                // Add audit log
                var categoryAuditLog = new RecordAuditLog
                {
                    ControllerName = "Category",
                    MethodName = "UpdateCategory",
                    TransactionId = currentCategory.Id,
                    TransactionNo = currentCategory.Name,
                    Description = await GetAuditDescription(currentCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(categoryAuditLog);

                // Response
                return ApiResponse<UpdateCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                _logger.Error($"CategoryService/UpdateCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescription(int id)
        {
            var Category = await _CategoryRepository.GetCategory(id);
            var recordAuditLogCategory = _mapper.Map<RecordAuditLogCategory>(Category);
            return JsonSerializer.Serialize(recordAuditLogCategory);
        }

        public async Task<ApiResponse<DeleteCategoryResponse>> DeleteCategory(DeleteCategoryRequest req)
        {
            try
            {
                // Check if category exists
                var getCategory = await _CategoryRepository.GetCategory(req.Id);
                if (getCategory == null)
                {
                    _logger.Error($"CategoryService/DeleteCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.NotFound.Value()}', ErrorMessage:'{ApiResponseStatus.NotFound.Description()}'");
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                if (getCategory == null)
                {
                    _logger.Error($"CategoryService/DeleteCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.AlreadyDeleted.Value()}', ErrorMessage:'{ApiResponseStatus.AlreadyDeleted.Description()}'");
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    _logger.Error($"CategoryService/DeleteCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{ApiResponseStatus.Unauthorized.Value()}', ErrorMessage:'{ApiResponseStatus.Unauthorized.Description()}'");
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // update category type
                var category = _mapper.Map<Category>(req);
                category.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyName(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                category.UserId = userId;
                category.ModifiedBy = userId;
                category.ModifiedDate = GlobalFunction.GetCurrentDateTime();
                var currentCategory = await _CategoryRepository.DeleteCategory(category);
                var categoryRes = _mapper.Map<DeleteCategoryResponse>(currentCategory);

                // Add audit log
                var categoryAuditLog = new RecordAuditLog
                {
                    ControllerName = "Category",
                    MethodName = "DeleteCategory",
                    TransactionId = currentCategory.Id,
                    TransactionNo = currentCategory.Name,
                    Description = await GetAuditDescription(currentCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogService.RecordAuditLog(categoryAuditLog);

                // Response
                return ApiResponse<DeleteCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                _logger.Error($"CategoryService/DeleteCategory, Param:{JsonSerializer.Serialize(req)}, ErrorCode:'{e.HResult}', ErrorMessage:'{e.Message}'");
                return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
