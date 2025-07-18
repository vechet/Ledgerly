using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.TransactionType
{
    public class CreateTransactionTypeResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;
    }
}
