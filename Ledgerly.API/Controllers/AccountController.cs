using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models.DTOs.Account;
using Ledgerly.API.Models.DTOs.Transaction;
using Ledgerly.API.Services;
using Ledgerly.API.Services.Interfaces;
using Ledgerly.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ledgerly.API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_CREATE))]
        [HttpPost("v1/account/create-account")]
        public async Task<ApiResponse<CreateAccountResponse>> CreateAccount([FromBody] CreateAccountRequest req)
        {
            return await _accountService.CreateAccount(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_VIEW))]
        [HttpPost("v1/account/get-accounts")]
        public async Task<ApiResponse<GetAccountsResponse>> GetAccounts([FromBody] PaginationRequest req)
        {
            return await _accountService.GetAccounts(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_UPDATE))]
        [HttpPost("v1/account/update-account")]
        public async Task<ApiResponse<UpdateAccountResponse>> UpdateAccount([FromBody] UpdateAccountRequest req)
        {
            return await _accountService.UpdateAccount(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_VIEW))]
        [HttpPost("v1/account/get-account")]
        public async Task<ApiResponse<GetAccountResponse>> GetAccount([FromBody] GetAccountRequest req)
        {
            return await _accountService.GetAccount(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_DELETE))]
        [HttpPost("v1/account/delete-account")]
        public async Task<ApiResponse<DeleteAccountResponse>> DeleteAccount([FromBody] DeleteAccountRequest req)
        {
            return await _accountService.DeleteAccount(req);
        }
    }
}
