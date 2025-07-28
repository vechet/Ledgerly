using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.TransactionType
{
    public class CreateAuditLogRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public short StatusId { get; set; }
    }
}
