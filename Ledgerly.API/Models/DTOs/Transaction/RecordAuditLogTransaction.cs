using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class RecordAuditLogTransaction
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string TransactionNo { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!; // FK to AspNetUsers

        //[Required]
        //public string UserName { get; set; } = null!;

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        [Required]
        public string TransactionTypeName { get; set; } = null!;

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

        public string? Notes { get; set; }

        [Required]
        public short StatusId { get; set; }

        [Required]
        public string StatusName { get; set; } = null!;

        [Required]
        public string CreatedBy { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string? ModifiedBy { get; set; }

        [Required]
        public DateTime? ModifiedDate { get; set; }
    }
}
