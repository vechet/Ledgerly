using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Ledgerly.API.Helpers
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            // Check if the user has the exact permission claim
            var hasPermission = context.User.Claims
                .Where(c => c.Type == "Permission")
                .Any(c => c.Value == requirement.PermissionName);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }

}
