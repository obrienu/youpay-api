using System;
using Microsoft.EntityFrameworkCore;
using Youpay.API.Models;

namespace Youpay.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankingDetails>()
                .Property(p => p.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<User>()
            .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Transaction>()
            .Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<BankingDetails>()
                .Property(p => p.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<User>()
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Transaction>()
            .Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Transaction>()
                .Property(p => p.HasPaid)
                .HasDefaultValue(false);

            modelBuilder.Entity<Transaction>()
                .Property(p => p.IsCanceled)
                .HasDefaultValue(false);

            modelBuilder.Entity<Transaction>()
                .Property(p => p.HasShipped)
                .HasDefaultValue(false);

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Delivered)
                .HasDefaultValue(false);

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Completed)
                .HasDefaultValue(false);

            modelBuilder.Entity<Transaction>()
                .Property(p => p.HasIssue)
                .HasDefaultValue(false);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<BankingDetails> BankingDetails { get; set; }
    }
}