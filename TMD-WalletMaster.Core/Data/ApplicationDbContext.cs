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

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Goal> Goals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=WIN-O9A7KB9ID4M\\SQLEXPRESS;Database=FinanceDB;Trusted_Connection=True;",
                    b => b.MigrationsAssembly("TMD-WalletMaster.Core"));
            }
        }

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