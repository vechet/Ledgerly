using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.TransactionType;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionTypeService _transactionTypeService;

        public TransactionTypeController(ITransactionTypeService transactionTypeService)
        {
            _transactionTypeService = transactionTypeService;
        }

        [HttpPost("v1/transaction-type/create-transaction-type")]
        public async Task<ApiResponse<CreateTransactionTypeResponse>> CreateBrand([FromForm] CreateTransactionTypeRequest req)
        {
            return await _transactionTypeService.CreateTransactionType(req);
        }

        [HttpPost("v1/transaction-type/get-transaction-types")]
        public async Task<ApiResponse<GetTransactionTypesResponse>> GetBrands([FromBody] PaginationRequest req)
        {
            return await _transactionTypeService.GetTransactionTypes(req);
        }

        [HttpPost("v1/transaction-type/update-transaction-type")]
        public async Task<ApiResponse<UpdateTransactionTypeResponse>> UpdateBrand([FromForm] UpdateTransactionTypeRequest req)
        {
            return await _transactionTypeService.UpdateTransactionType(req);
        }

        [HttpPost("v1/transaction-type/get-transaction-type")]
        public async Task<ApiResponse<GetTransactionTypeResponse>> GetBrand([FromBody] GetTransactionTypeRequest req)
        {
            return await _transactionTypeService.GetTransactionType(req);
        }
    }
}
