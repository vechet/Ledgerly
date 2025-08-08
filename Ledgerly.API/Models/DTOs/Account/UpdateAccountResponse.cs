using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Account
{
    public class UpdateAccountResponse
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Currency { get; set; } = null!;

        public string? Memo { get; set; }

        [Required]
        public short StatusId { get; set; }
    }
}
