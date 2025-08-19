using Ledgerly.API.Enums;
using Ledgerly.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Data
{
    public class LedgerlyAuthDbContext : IdentityDbContext
    {
        public LedgerlyAuthDbContext(DbContextOptions<LedgerlyAuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var systemAdminRoleId = "role-system-admin-id";
            var userId = "system-admin-id";
            var roleSystemAdmin = EnumRoles.ROLE_SYSTEM_ADMIN.ToString();
            var roleAdmin = EnumRoles.ROLE_ADMIN.ToString();
            var roleUser = EnumRoles.ROLE_USER.ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = systemAdminRoleId, Name = roleSystemAdmin, NormalizedName = roleSystemAdmin },
                new IdentityRole { Name = roleAdmin, NormalizedName = roleAdmin },
                new IdentityRole { Name = roleUser, NormalizedName = roleUser }
                );

            builder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = userId,
                    UserName = "systemadmin",
                    NormalizedUserName = "SYSTEMADMIN",
                    Email = "systemadmin@gmail.com.com",
                    NormalizedEmail = "SYSTEMADMIN@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "chet@12345"),
                    LockoutEnabled = true
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = userId,
                    RoleId = systemAdminRoleId
                }
            );
        }

    }
}
