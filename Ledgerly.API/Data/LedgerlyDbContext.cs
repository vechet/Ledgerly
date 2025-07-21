using Ledgerly.API.Models.Domains;
using Ledgerly.Models;
using Microsoft.EntityFrameworkCore;

namespace Udemy.Data
{
    public class LedgerlyDbContext : DbContext
    {
        public LedgerlyDbContext(DbContextOptions<LedgerlyDbContext> options) : base(options) { }

        public DbSet<Transaction> Transaction { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Account> Account { get; set; }

        public DbSet<TransactionType> TransactionType { get; set; }

        public DbSet<CategoryType> CategoryType { get; set; }

        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<Status> Status { get; set; }
    }
}
