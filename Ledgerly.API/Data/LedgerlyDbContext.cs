using Ledgerly.API.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Data
{
    public class LedgerlyDbContext : DbContext
    {
        public LedgerlyDbContext(DbContextOptions<LedgerlyDbContext> options) : base(options) { }

        public DbSet<Transaction> transactions { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Account> accounts { get; set; }

        public DbSet<TransactionType> transactionTypes { get; set; }

        public DbSet<CategoryType> categoryTypes { get; set; }
    }
}
