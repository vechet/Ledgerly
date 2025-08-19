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

        public DbSet<AuditLog> AuditLog { get; set; }

        public DbSet<GlobalParam> GlobalParam { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var globalParamCategoryStatusId = 1;
            var systemAdminRoleId = "role-system-admin-id";

            // Set the collation for the entire database
            modelBuilder.UseCollation("SQL_Latin1_General_CP850_BIN");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasOne(e => e.GlobalParam).WithMany(p => p.Categories)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_GlobalParam_StatusId");

                entity.HasOne(e => e.Parent).WithMany(p => p.Children)
                    .HasForeignKey(e => e.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_Parent_ParentId");

                //Seed a default record for Category
                entity.HasData(new Category
                {
                    Id = 1,
                    Name = "Main",
                    StatusId = globalParamCategoryStatusId,
                    UserId = systemAdminRoleId,
                    CreatedBy = "1",
                    CreatedDate = new DateTime(2025, 07, 22),
                    IsSystemValue = true,
                });
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasOne(e => e.GlobalParam).WithMany(p => p.Transactions)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_GlobalParam_StatusId");

                entity.HasOne(e => e.Category).WithMany(p => p.Transactions)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Category_CategoryId");

                entity.HasOne(e => e.Account).WithMany(p => p.Transactions)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Account_AccountId");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.HasOne(e => e.GlobalParam).WithMany(p => p.Accounts)
                    .HasForeignKey(e => e.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Status_StatusId");
            });

            modelBuilder.Entity<GlobalParam>(entity =>
            {
                entity.ToTable("GlobalParam");

                // Seed a default record for GlobalParam
                entity.HasData(new GlobalParam
                {
                    Id = globalParamCategoryStatusId,
                    Name = "Normal",
                    KeyName = "Normal",
                    Type = "CategoryxxxStatus",
                });
                entity.HasData(new GlobalParam
                {
                    Id = 2,
                    Name = "Deleted",
                    KeyName = "Deleted",
                    Type = "CategoryxxxStatus",
                });
                entity.HasData(new GlobalParam
                {
                    Id = 3,
                    Name = "Normal",
                    KeyName = "Normal",
                    Type = "TransactionxxxStatus",
                });
                entity.HasData(new GlobalParam
                {
                    Id = 4,
                    Name = "Deleted",
                    KeyName = "Deleted",
                    Type = "TransactionxxxStatus",
                });
                entity.HasData(new GlobalParam
                {
                    Id = 5,
                    Name = "Normal",
                    KeyName = "Normal",
                    Type = "AccountxxxStatus",
                });
                entity.HasData(new GlobalParam
                {
                    Id = 6,
                    Name = "Deleted",
                    KeyName = "Deleted",
                    Type = "AccountxxxStatus",
                });
            });

        }

    }
}
