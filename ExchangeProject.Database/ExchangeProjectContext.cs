using ExchangeProject.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ExchangeProject.Database
{
    public class ExchangeProjectContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public DbSet<ExchangeDate> ExchangeDates { get; set; }

        public ExchangeProjectContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ExchangeProject;Trusted_Connection=True;MultipleActiveResultSets=true");
            optionsBuilder.UseLazyLoadingProxies();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Currency entity
            modelBuilder.Entity<Currency>()
                .HasKey(c => c.Code);
            modelBuilder.Entity<Currency>()
                .Property(c => c.Amount)
                .IsRequired();

            modelBuilder.Entity<ExchangeDate>()
                .HasKey(e => e.Date);

            // Configure the ExchangeRate entity
            modelBuilder.Entity<ExchangeRate>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<ExchangeRate>()
                .Property(e => e.Rate)
                //.HasColumnType("decimal(10,4)")
                .IsRequired();


            // Configure the foreign keys
            modelBuilder.Entity<ExchangeRate>()
                .HasOne(e => e.Currency)
                .WithMany()
                .IsRequired();
            modelBuilder.Entity<ExchangeRate>()
                .HasOne(e => e.ExchangeDate)
                .WithMany()
                .IsRequired();
        }
    }
}
