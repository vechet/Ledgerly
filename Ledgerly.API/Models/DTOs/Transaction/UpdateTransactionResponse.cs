using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class UpdateTransactionResponse
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
        public int AccountId { get; set; }

        public string? Memo { get; set; }

        [Required]
        public string Type { get; set; } = null!;
    }
}
