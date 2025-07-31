using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
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

        [HttpPost("v1/transaction/create-transaction")]
        public async Task<ApiResponse<CreateTransactionResponse>> CreateBrand([FromBody] CreateTransactionRequest req)
        {
            return await _transactionService.CreateTransaction(req);
        }

        [HttpPost("v1/transaction/get-transactions")]
        public async Task<ApiResponse<GetTransactionsResponse>> GetBrands([FromBody] PaginationRequest req)
        {
            return await _transactionService.GetTransactions(req);
        }

        [HttpPost("v1/transaction/update-transaction")]
        public async Task<ApiResponse<UpdateTransactionResponse>> UpdateBrand([FromBody] UpdateTransactionRequest req)
        {
            return await _transactionService.UpdateTransaction(req);
        }

        [HttpPost("v1/transaction/get-transaction")]
        public async Task<ApiResponse<GetTransactionResponse>> GetBrand([FromBody] GetTransactionRequest req)
        {
            return await _transactionService.GetTransaction(req);
        }
    }
}
