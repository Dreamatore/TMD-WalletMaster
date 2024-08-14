using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Models;

namespace TMD_WalletMaster.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Конструктор по умолчанию для использования в миграциях
        public ApplicationDbContext() : base()
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(e => e.CurrentAmount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.TargetAmount)
                    .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");
            });
        }
    }
}