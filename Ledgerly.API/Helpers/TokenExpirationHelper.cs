namespace Ledgerly.API.Helpers
{
    public class TokenExpirationHelper
    {
        public static DateTime AccessTokenExpirationHandler(IConfiguration config)
        {
            var accessTokenExpirationType = config["Jwt:AccessTokenExpirationType"];
            var accessTokenExpiration = config["Jwt:AccessTokenExpiration"];
            var expiration = DateTime.Now;
            if (accessTokenExpirationType.Contains("s"))
            {
                expiration = expiration.AddSeconds(Convert.ToInt32(accessTokenExpiration));
            }

            if (accessTokenExpirationType.Contains("m"))
            {
                expiration = expiration.AddMinutes(Convert.ToInt32(accessTokenExpiration));
            }

            if (accessTokenExpirationType.Contains("h"))
            {
                expiration = expiration.AddHours(Convert.ToInt32(accessTokenExpiration));
            }

            if (accessTokenExpirationType.Contains("d"))
            {
                expiration = expiration.AddDays(Convert.ToInt32(accessTokenExpiration));
            }
            return expiration;
        }
    }
}
