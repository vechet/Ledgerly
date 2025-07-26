using Ledgerly.API.Services.Interfaces;
using System.Security.Claims;

namespace Ledgerly.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserId()
            => _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //?? throw new UnauthorizedAccessException("User not authenticated.");
        public string? GetUserName()
            => _contextAccessor.HttpContext?.User.Identity?.Name;
    }

}
