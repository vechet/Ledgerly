using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.TransactionType
{
    public class CreateTransactionTypeRequest
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
