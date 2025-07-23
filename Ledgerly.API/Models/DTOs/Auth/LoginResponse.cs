namespace Ledgerly.API.Models.DTOs.User
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public int ExpiresIn { get; set; }
    }
}
