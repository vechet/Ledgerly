using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.Account
{
    public class DeleteAccountRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
