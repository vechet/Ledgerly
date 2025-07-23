namespace Ledgerly.API.Models
{
    public class AccessTokenResponse
    {
        public string AccessToken { get; set; } = null!;
        public int ExpiresIn { get; set; }
    }
}
