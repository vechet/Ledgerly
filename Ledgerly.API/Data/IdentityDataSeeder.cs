using Ledgerly.API.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Ledgerly.API.Data
{
    public static class IdentityDataSeeder
    {
        private static readonly Dictionary<EnumRoles, EnumPermissions[]> RolePermissionMap = new()
        {
            [EnumRoles.ROLE_SYSTEM_ADMIN] = Enum.GetValues(typeof(EnumPermissions)).Cast<EnumPermissions>().ToArray(), // full access
            [EnumRoles.ROLE_ADMIN] = new[]
            {
                EnumPermissions.ACCOUNT_VIEW,
                EnumPermissions.ACCOUNT_UPDATE,
                EnumPermissions.CATEGORY_VIEW,
                EnumPermissions.TRANSACTION_VIEW
            },
            [EnumRoles.ROLE_USER] = Enum.GetValues(typeof(EnumPermissions)).Cast<EnumPermissions>().ToArray(),
        };

        public static async Task SeedRolesAndClaimsAsync(RoleManager<IdentityRole> roleManager)
        {
            var claimType = EnumClaimTypes.PERMISSIONS.ToString();

            foreach (var entry in RolePermissionMap)
            {
                var roleName = entry.Key.ToString();
                var permissions = entry.Value;

                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                var role = await roleManager.FindByNameAsync(roleName);
                var existingClaims = await roleManager.GetClaimsAsync(role);
                var existingValues = existingClaims
                    .Where(c => c.Type == claimType)
                    .Select(c => c.Value)
                    .ToHashSet();

                foreach (var perm in permissions)
                {
                    var value = perm.ToString();
                    if (!existingValues.Contains(value))
                    {
                        await roleManager.AddClaimAsync(role, new Claim(claimType, value));
                    }
                }
            }
        }

    }

}
