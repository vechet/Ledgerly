using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class CreateTransactionRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public short Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        public string? Memo { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public short StatusId { get; set; }
    }
}
