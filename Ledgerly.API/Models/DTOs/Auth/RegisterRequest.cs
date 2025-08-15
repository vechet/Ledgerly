using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.User
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        //public string[] Roles { get; set; }
    }
}
