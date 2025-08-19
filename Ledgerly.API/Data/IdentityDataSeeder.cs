using Ledgerly.API.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Ledgerly.API.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndClaimsAsync(RoleManager<IdentityRole> roleManager)
        {
            var adminRoleName = EnumRoles.ROLE_SYSTEM_ADMIN.ToString();

            if (!await roleManager.RoleExistsAsync(adminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));
            }

            var role = await roleManager.FindByNameAsync(adminRoleName);
            var existingClaims = await roleManager.GetClaimsAsync(role);
            var claimTypesPerm = EnumClaimTypes.PERMISSIONS.ToString();
            var existingValues = existingClaims
                .Where(c => c.Type == claimTypesPerm)
                .Select(c => c.Value)
                .ToHashSet();

            foreach (EnumPermissions perm in Enum.GetValues(typeof(EnumPermissions)))
            {
                var value = perm.ToString();
                if (!existingValues.Contains(value))
                {
                    await roleManager.AddClaimAsync(role, new Claim(claimTypesPerm, value));
                }
            }
        }
    }

}
