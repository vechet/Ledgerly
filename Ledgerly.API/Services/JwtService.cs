using Ledgerly.API.Enums;
using Ledgerly.API.Helpers;
using Ledgerly.API.Models;
using Ledgerly.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Udemy.Data;

namespace Ledgerly.Services
{
    public class JwtService(IConfiguration configuration,
         UserManager<IdentityUser> userManager,
         RoleManager<IdentityRole> roleManager) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public List<Claim> GetAllClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
            };

            var roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult();

            foreach (var roleName in roles)
            {
                // Add role claim
                claims.Add(new Claim(ClaimTypes.Role, roleName));

                // Add permission claims from role
                var role = _roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
                var roleClaims = _roleManager.GetClaimsAsync(role).GetAwaiter().GetResult();

                var claimTypesPerm = EnumClaimTypes.PERMISSIONS.ToString();
                var permissionClaims = roleClaims
                    .Where(c => c.Type == claimTypesPerm)
                    .Select(c => new Claim(claimTypesPerm, c.Value));

                claims.AddRange(permissionClaims);
            }

            return claims;
        }

        public AccessTokenResponse GenerateToken(IdentityUser user, List<string> roles)
        {
            // Create claims
            var claims = GetAllClaims(user);
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessExpiration = TokenExpirationHelper.AccessTokenExpirationHandler(_configuration);
            var jwtAccessToken = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: accessExpiration,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
            var accessExpiresIn = (int)(accessExpiration - DateTime.Now).TotalSeconds;

            return new AccessTokenResponse
            {
                AccessToken = accessToken,
                ExpiresIn = accessExpiresIn
            };
        }
    }
}
