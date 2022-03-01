using System;
using Microsoft.EntityFrameworkCore;
using Pozitive.Entities;
using Pozitive.Entities.Enums;

namespace Positive.SqlDbContext
{
    public sealed class PozitiveSqlContext : DbContext
    {
        public DbSet<Person> Users { get; set; }
        public DbSet<Document> Documents { get; set; }

        public PozitiveSqlContext(DbContextOptions<PozitiveSqlContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Person>()
                .Property(p => p.Status)
                .HasConversion(
                    v => (int)v,
                    v => (UserStatus)v);
        }
    }
}
