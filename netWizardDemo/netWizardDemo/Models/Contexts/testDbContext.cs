using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace netWizardDemo.Models.Contexts
{
    public class testDbContext : DbContext
    {
        public testDbContext() : base("testDb")
        {

        }
        public DbSet<MUser> Users { get; set; }
        public DbSet<MApplication> Applications { get; set; }
        public DbSet<MAddress> Address { get; set; }
    }
}