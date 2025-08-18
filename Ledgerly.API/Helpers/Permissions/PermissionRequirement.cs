namespace Ledgerly.API.Helpers.Permissions
{
    using Ledgerly.API.Enums;
    using Microsoft.AspNetCore.Authorization;

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }

}
