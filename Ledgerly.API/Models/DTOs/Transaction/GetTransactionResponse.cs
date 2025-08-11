using AutoMapper.Configuration.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class GetTransactionResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Date => TransactionDate.ToString("yyyy-MM-dd HH:mm:ss");

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
    }
}
