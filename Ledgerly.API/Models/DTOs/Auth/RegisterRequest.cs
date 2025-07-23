using System.ComponentModel.DataAnnotations;

namespace Ledgerly.API.Models.DTOs.User
{
    public class RegisterRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string[] Roles { get; set; }
    }
}
