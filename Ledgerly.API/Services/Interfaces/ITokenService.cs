using Microsoft.AspNetCore.Identity;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
