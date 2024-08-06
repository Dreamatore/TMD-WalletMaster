using Microsoft.EntityFrameworkCore;
using TMD_WalletMaster.Core.Models;
using Microsoft.Extensions.Configuration;

namespace TMD_WalletMaster.Core.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
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