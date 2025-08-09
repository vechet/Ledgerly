using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Transaction
{
    public class DeleteTransactionRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
