using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain
{

    public class SampleContext : DbContext
    {
        public SampleContext() : base("DefaultConnection")
        {
            Database.SetInitializer<SampleContext>(new CreateDatabaseIfNotExists<SampleContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Doughnut> Doughnuts { get; set; }
        public DbSet<Flavor> Flavors { get; set; }
    }
}
