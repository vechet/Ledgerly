using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.Domains;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.Category;
using Ledgerly.API.Repositories;
using Ledgerly.API.Repositories.Interfaces;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Ledgerly.Models;
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
        IAuditLogRepository auditLogRepository,
        IGlobalParamRepository globalParamRepository) : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository = CategoryRepository;
        private readonly IMapper _mapper = mapper;
        private readonly LedgerlyDbContext _db = db;
        private readonly IUserService _currentUserService = currentUserService;
        private readonly IAuditLogRepository _auditLogRepository = auditLogRepository;
        private readonly IGlobalParamRepository _globalParamRepository = globalParamRepository;

        public async Task<ApiResponse<CreateCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("CategoryService", "CreateCategory", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // mapping dto to entity
                var category = _mapper.Map<Category>(req);
                category.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                category.UserId = userId;
                category.CreatedBy = userId;
                category.CreatedDate = GlobalFunction.GetCurrentDateTime();

                // add new transaction
                var newCategory = await _CategoryRepository.CreateCategoryAsync(category);

                // Add audit log
                var categoryAuditLog = new AuditLog
                {
                    ControllerName = "Category",
                    MethodName = "Create",
                    TransactionId = newCategory.Id,
                    TransactionNo = newCategory.Name,
                    Description = await GetAuditDescriptionAsync(newCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogRepository.CreateAuditLogAsync(categoryAuditLog);

                // saving
                await _db.SaveChangesAsync();

                // Response
                var categoryRes = _mapper.Map<CreateCategoryResponse>(newCategory);
                return ApiResponse<CreateCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("CategoryService", "CreateCategory", req, e.HResult, e.Message);
                return ApiResponse<CreateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoryResponse>> GetCategoryAsync(GetCategoryRequest req)
        {
            try
            {
                // Check if category exists
                var getCategory = await _CategoryRepository.GetCategoryAsync(req.Id);
                if (getCategory == null)
                {
                    LogHelper.Info("CategoryService", "GetCategory", req, ApiResponseStatus.NotFound);
                    return ApiResponse<GetCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Response
                var categoryRes = _mapper.Map<GetCategoryResponse>(getCategory);
                return ApiResponse<GetCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("CategoryService", "GetCategory", req, e.HResult, e.Message);
                return ApiResponse<GetCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<GetCategoriesResponse>> GetCategoriesAsync(PaginationRequest req)
        {
            try
            {
                //get userId
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("TransactionService", "GetCategories", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<GetCategoriesResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                var isSystemAdminUser = _currentUserService.IsSystemAdminUser();
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Normal.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                var query = _db.Category.Where(x => x.StatusId == status && (x.IsSystemValue || (x.UserId == userId && !x.IsSystemValue) || isSystemAdminUser)).AsQueryable();

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
                LogHelper.Info("CategoryService", "GetCategories", req, e.HResult, e.Message);
                return ApiResponse<GetCategoriesResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<ApiResponse<UpdateCategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest req)
        {
            try
            {
                // Check if category exists
                var getCategory = await _CategoryRepository.GetCategoryAsync(req.Id);
                if (getCategory == null)
                {
                    LogHelper.Info("CategoryService", "UpdateCategory", req, ApiResponseStatus.NotFound);
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                if (getCategory == null)
                {
                    LogHelper.Info("CategoryService", "UpdateCategory", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("CategoryService", "UpdateCategory", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // mapping dto to entity
                var category = _mapper.Map<Category>(req);
                category.UserId = userId;
                category.ModifiedBy = userId;
                category.ModifiedDate = GlobalFunction.GetCurrentDateTime();

                // update category
                var currentCategory = await _CategoryRepository.UpdateCategoryAsync(category);

                // Add audit log
                var categoryAuditLog = new AuditLog
                {
                    ControllerName = "Category",
                    MethodName = "Update",
                    TransactionId = currentCategory.Id,
                    TransactionNo = currentCategory.Name,
                    Description = await GetAuditDescriptionAsync(currentCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogRepository.CreateAuditLogAsync(categoryAuditLog);

                // saving
                await _db.SaveChangesAsync();

                // Response
                var categoryRes = _mapper.Map<UpdateCategoryResponse>(currentCategory);
                return ApiResponse<UpdateCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("CategoryService", "UpdateCategory", req, e.HResult, e.Message);
                return ApiResponse<UpdateCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }

        public async Task<string> GetAuditDescriptionAsync(int id)
        {
            var Category = await _CategoryRepository.GetCategoryAsync(id);
            var recordAuditLogCategory = _mapper.Map<RecordAuditLogCategory>(Category);
            return JsonSerializer.Serialize(recordAuditLogCategory);
        }

        public async Task<ApiResponse<DeleteCategoryResponse>> DeleteCategoryAsync(DeleteCategoryRequest req)
        {
            try
            {
                var getCategory = await _CategoryRepository.GetCategoryAsync(req.Id);

                // Check if category exists
                if (getCategory == null)
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.NotFound);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.NotFound);
                }

                // Check if the account record is already deleted
                var status = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                if (getCategory == null)
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.AlreadyDeleted);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.AlreadyDeleted);
                }

                // prevent delete default category
                if (getCategory.IsSystemValue)
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.CannotDeleteDefaultCategory);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.CannotDeleteDefaultCategory);
                }

                //get user id
                var userId = _currentUserService.GetUserId();
                if (userId == null)
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.Unauthorized);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.Unauthorized);
                }

                // allow to delete only category that created by own user
                if (!getCategory.IsSystemValue && getCategory.UserId != userId)
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.CannotDeleteOtherUserCategory);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.CannotDeleteOtherUserCategory);
                }

                // prevent delete own user category, if any transactions are linked to this category
                if (await _CategoryRepository.HasTransactionsLinkedAsync(req.Id, userId))
                {
                    LogHelper.Info("CategoryService", "DeleteCategory", req, ApiResponseStatus.HasTransactionsLinkedAsync);
                    return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.HasTransactionsLinkedAsync);
                }

                // mapping dto to entity
                var category = _mapper.Map<Category>(req);
                category.StatusId = await _globalParamRepository.GetGlobalParamIdByKeyNameAsync(EnumGlobalParam.Deleted.ToString(), EnumGlobalParamType.CategoryxxxStatus.ToString());
                category.UserId = userId;
                category.ModifiedBy = userId;
                category.ModifiedDate = GlobalFunction.GetCurrentDateTime();

                // update category
                var currentCategory = await _CategoryRepository.DeleteCategoryAsync(category);

                // Add audit log
                var categoryAuditLog = new AuditLog
                {
                    ControllerName = "Category",
                    MethodName = "Delete",
                    TransactionId = currentCategory.Id,
                    TransactionNo = currentCategory.Name,
                    Description = await GetAuditDescriptionAsync(currentCategory.Id),
                    CreatedBy = userId,
                    CreatedDate = GlobalFunction.GetCurrentDateTime(),
                };
                await _auditLogRepository.CreateAuditLogAsync(categoryAuditLog);

                // saving
                await _db.SaveChangesAsync();

                // Response
                var categoryRes = _mapper.Map<DeleteCategoryResponse>(currentCategory);
                return ApiResponse<DeleteCategoryResponse>.Success(categoryRes);
            }
            catch (Exception e)
            {
                LogHelper.Info("CategoryService", "DeleteCategory", req, e.HResult, e.Message);
                return ApiResponse<DeleteCategoryResponse>.Failure(ApiResponseStatus.InternalError);
            }
        }
    }
}
