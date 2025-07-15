using Ledgerly.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Data
{
    public class LedgerlyDbContext : DbContext
    {
        public LedgerlyDbContext(DbContextOptions<LedgerlyDbContext> options) : base(options) { }

        public DbSet<Transaction> transactions { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Account> accounts { get; set; }
    }
}
