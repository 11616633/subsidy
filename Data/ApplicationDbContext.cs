using Microsoft.EntityFrameworkCore;
using SubsidyReconciliation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set the connection timeout here
            // in seconds
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<SubsidyRecord> SubsidyRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubsidyRecord>().HasNoKey();

            // Other configurations if needed

            base.OnModelCreating(modelBuilder);
        }
    }
}
