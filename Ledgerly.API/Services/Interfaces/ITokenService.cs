using Ledgerly.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Ledgerly.API.Services.Interfaces
{
    public interface ITokenService
    {
        public AccessTokenResponse CreateAccessToken(IdentityUser user, List<string> roles);
    }
}
