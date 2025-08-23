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
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IBrandService _brandService;

        public AccountController(IAccountService accountService, IBrandService brandService)
        {
            _accountService = accountService;
            _brandService = brandService;
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_CREATE))]
        [HttpPost("v1/account/create-account")]
        public async Task<ApiResponse<CreateAccountResponse>> CreateAccountAsync([FromBody] CreateAccountRequest req)
        {
            //return await _accountService.CreateAccountAsync(req);
            return await _brandService.CreateAccountAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_VIEW))]
        [HttpPost("v1/account/get-accounts")]
        public async Task<ApiResponse<GetAccountsResponse>> GetAccountsAsync([FromBody] PaginationRequest req)
        {
            return await _accountService.GetAccountsAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_UPDATE))]
        [HttpPost("v1/account/update-account")]
        public async Task<ApiResponse<UpdateAccountResponse>> UpdateAccountAsync([FromBody] UpdateAccountRequest req)
        {
            return await _accountService.UpdateAccountAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_VIEW))]
        [HttpPost("v1/account/get-account")]
        public async Task<ApiResponse<GetAccountResponse>> GetAccountAsync([FromBody] GetAccountRequest req)
        {
            return await _accountService.GetAccountAsync(req);
        }

        [Authorize(Policy = nameof(EnumPermissions.ACCOUNT_DELETE))]
        [HttpPost("v1/account/delete-account")]
        public async Task<ApiResponse<DeleteAccountResponse>> DeleteAccountAsync([FromBody] DeleteAccountRequest req)
        {
            return await _accountService.DeleteAccountAsync(req);
        }
    }
}
