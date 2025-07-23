using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.User
{
    public class RegisterResponse
    {
        [Required]
        public string Username { get; set; } = null!;
    }
}
