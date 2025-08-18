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
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("v1/transaction/create-transaction")]
        public async Task<ApiResponse<CreateTransactionResponse>> CreateTransaction([FromBody] CreateTransactionRequest req)
        {
            return await _transactionService.CreateTransaction(req);
        }

        [HttpPost("v1/transaction/get-transactions")]
        public async Task<ApiResponse<GetTransactionsResponse>> GetTransactions([FromBody] PaginationRequest req)
        {
            return await _transactionService.GetTransactions(req);
        }

        [HttpPost("v1/transaction/update-transaction")]
        public async Task<ApiResponse<UpdateTransactionResponse>> UpdateTransaction([FromBody] UpdateTransactionRequest req)
        {
            return await _transactionService.UpdateTransaction(req);
        }

        [HttpPost("v1/transaction/get-transaction")]
        public async Task<ApiResponse<GetTransactionResponse>> GetTransaction([FromBody] GetTransactionRequest req)
        {
            return await _transactionService.GetTransaction(req);
        }

        [HttpPost("v1/transaction/delete-transaction")]
        public async Task<ApiResponse<DeleteTransactionResponse>> DeleteTransaction([FromBody] DeleteTransactionRequest req)
        {
            return await _transactionService.DeleteTransaction(req);
        }
    }
}
