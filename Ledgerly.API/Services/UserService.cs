using Ledgerly.API.Models.DTOs.User;
using Ledgerly.API.Services.Interfaces;
using System.Security.Claims;

namespace Ledgerly.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public UserInfo GetUserInfo()
        {
            var getUserInfo = _contextAccessor.HttpContext.User;
            var userInfo = new UserInfo
            {
                Id = getUserInfo.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = getUserInfo.FindFirst(ClaimTypes.Name)?.Value,
                Email = getUserInfo.FindFirst(ClaimTypes.Email)?.Value,
                Phone = getUserInfo.FindFirst(ClaimTypes.MobilePhone)?.Value,
            };
            return userInfo;
        }

        public string GetUserId()
        {
            return _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //?? throw new UnauthorizedAccessException("User not authenticated.");
        }
        public string? GetUserName()
        {
            return _contextAccessor.HttpContext?.User.Identity?.Name;
        }

        public string GetRole()
        {
            return _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
        }
    }
}
