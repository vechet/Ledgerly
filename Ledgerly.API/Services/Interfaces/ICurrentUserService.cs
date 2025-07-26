namespace Ledgerly.API.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string GetUserId();
        string? GetUserName(); // optional
    }
}
