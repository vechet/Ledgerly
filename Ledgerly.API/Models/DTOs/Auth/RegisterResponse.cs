using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.User
{
    public class RegisterResponse
    {
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }
    }
}
