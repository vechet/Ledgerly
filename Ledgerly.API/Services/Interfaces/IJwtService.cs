using Ledgerly.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IJwtService
    {
        public AccessTokenResponse GenerateToken(IdentityUser user, List<string> roles);
    }
}
