using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize(Policy = nameof(EnumPermissions.TRANSACTION_CREATE))]
        [HttpPost("v1/transaction/create-transaction")]
        public async Task<ApiResponse<CreateTransactionResponse>> CreateTransactionAsync([FromBody] CreateTransactionRequest req)
        {
            return await _transactionService.CreateTransactionAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.TRANSACTION_VIEW))]
        [HttpPost("v1/transaction/get-transactions")]
        public async Task<ApiResponse<GetTransactionsResponse>> GetTransactionsAsync([FromBody] PaginationRequest req)
        {
            return await _transactionService.GetTransactionsAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.TRANSACTION_UPDATE))]
        [HttpPost("v1/transaction/update-transaction")]
        public async Task<ApiResponse<UpdateTransactionResponse>> UpdateTransactionAsync([FromBody] UpdateTransactionRequest req)
        {
            return await _transactionService.UpdateTransactionAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.TRANSACTION_VIEW))]
        [HttpPost("v1/transaction/get-transaction")]
        public async Task<ApiResponse<GetTransactionResponse>> GetTransactionAsync([FromBody] GetTransactionRequest req)
        {
            return await _transactionService.GetTransactionAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.TRANSACTION_DELETE))]
        [HttpPost("v1/transaction/delete-transaction")]
        public async Task<ApiResponse<DeleteTransactionResponse>> DeleteTransactionAsync([FromBody] DeleteTransactionRequest req)
        {
            return await _transactionService.DeleteTransactionAsync(req);
        }
    }
}
