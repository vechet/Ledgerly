using Ledgerly.API.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class GetTransactionsResponse
    {
        public List<TransactionsResponse> Transactions { get; set; }
        public PageInfo PageInfo { get; set; }

        public GetTransactionsResponse()
        { }

        public GetTransactionsResponse(List<TransactionsResponse> transactions, PageInfo pageInfo)
        {
            Transactions = transactions;
            PageInfo = pageInfo;
        }
    }

    public class TransactionsResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = null!;

        [Required]
        public int AccountId { get; set; }

        [Required]
        public string AccountName { get; set; } = null!;

        public string? Memo { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public short StatusId { get; set; }

        [Required]
        public string StatusName { get; set; } = null!;
    }
}
