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

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Name = "System Admin", NormalizedName = "ROLE_SYSTEM_ADMIN" },
                new IdentityRole { Name = "Admin", NormalizedName = "ROLE_ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "ROLE_USER" }
            );
        }

    }
}
