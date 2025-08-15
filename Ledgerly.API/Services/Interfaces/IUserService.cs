using Ledgerly.API.Models.DTOs.User;

namespace Ledgerly.API.Services.Interfaces
{
    public interface IUserService
    {
        UserInfo GetUserInfo();
        string GetUserId();
        string? GetUserName();
        string GetRole();
    }
}
