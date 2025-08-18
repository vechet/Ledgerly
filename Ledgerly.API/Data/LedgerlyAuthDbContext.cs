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

            var adminRoleId = "role-admin-id";
            var userId = "user-id";

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = adminRoleId, Name = "ROLE_SYSTEM_ADMIN", NormalizedName = "ROLE_SYSTEM_ADMIN" },
                new IdentityRole { Name = "ROLE_ADMIN", NormalizedName = "ROLE_ADMIN" },
                new IdentityRole { Name = "ROLE_USER", NormalizedName = "ROLE_USER" }
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
                    RoleId = adminRoleId
                }
            );
        }

    }
}
