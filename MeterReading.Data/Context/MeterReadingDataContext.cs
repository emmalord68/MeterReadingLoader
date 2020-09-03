using MeterReading.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeterReading.Data.Context
{
    public class MeterReadingDataContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Reading> Readings { get; set; }
        public MeterReadingDataContext(DbContextOptions<MeterReadingDataContext> options)
            : base(options)
        {
        }

        public MeterReadingDataContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reading>().HasKey(e => new { e.AccountId, e.MeterReadingDateTime });
            builder.Entity<Account>().HasKey(e => new { e.AccountId });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

