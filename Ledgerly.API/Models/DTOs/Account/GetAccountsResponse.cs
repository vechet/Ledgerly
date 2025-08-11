using System.ComponentModel.DataAnnotations;
using Ledgerly.API.Helpers;

namespace Ledgerly.API.Models.DTOs.Account
{
    public class GetAccountsResponse
    {
        public List<AccountsResponse> Accounts { get; set; }
        public PageInfo PageInfo { get; set; }

        public GetAccountsResponse()
        { }

        public GetAccountsResponse(List<AccountsResponse> accounts, PageInfo pageInfo)
        {
            Accounts = accounts;
            PageInfo = pageInfo;
        }
    }

    public class AccountsResponse
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Currency { get; set; } = null!;

        public string? Memo { get; set; }
    }
}
