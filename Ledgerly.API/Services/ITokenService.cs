using Microsoft.AspNetCore.Identity;

namespace Ledgerly.Services
{
    public interface ITokenService
    {
        public string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
