using Ledgerly.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Data
{
    public class LedgerlyAuthDbContext : IdentityDbContext
    {
        public LedgerlyAuthDbContext(DbContextOptions<LedgerlyAuthDbContext> options) : base(options) { }
    }
}
