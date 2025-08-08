using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _AccountService;

        public AccountController(IAccountService AccountService)
        {
            _AccountService = AccountService;
        }

        [HttpPost("v1/account/create-account")]
        public async Task<ApiResponse<CreateAccountResponse>> CreateAccount([FromBody] CreateAccountRequest req)
        {
            return await _AccountService.CreateAccount(req);
        }

        [HttpPost("v1/account/get-accounts")]
        public async Task<ApiResponse<GetAccountsResponse>> GetAccounts([FromBody] PaginationRequest req)
        {
            return await _AccountService.GetAccounts(req);
        }

        [HttpPost("v1/account/update-account")]
        public async Task<ApiResponse<UpdateAccountResponse>> UpdateAccount([FromBody] UpdateAccountRequest req)
        {
            return await _AccountService.UpdateAccount(req);
        }

        [HttpPost("v1/account/get-account")]
        public async Task<ApiResponse<GetAccountResponse>> GetAccount([FromBody] GetAccountRequest req)
        {
            return await _AccountService.GetAccount(req);
        }
    }
}
