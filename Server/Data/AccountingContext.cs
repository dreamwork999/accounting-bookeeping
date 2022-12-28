using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using AccountingBookkeeping.Server.Models.accounting;

namespace AccountingBookkeeping.Server.Data
{
    public partial class accountingContext : DbContext
    {
        public accountingContext()
        {
        }

        public accountingContext(DbContextOptions<accountingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<AccountingBookkeeping.Server.Models.accounting.Pricing> Pricings { get; set; }
    }
}