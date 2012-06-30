using System.Data.Entity;
using Zac.StandardCore.Models;

namespace Zac.StandardInfrastructure.EntityFramework
{
    public class StandardDbContext : DbContext
    {
        public StandardDbContext( string nameOrConnectionString )
            : base( nameOrConnectionString )
        {
        }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Entity<SiteMapNode>().ToTable( "App_SiteMapNode" );
            modelBuilder.Entity<SystemParameter>().ToTable( "App_SystemParameter" );
            base.OnModelCreating( modelBuilder );
        }

        public DbSet<SiteMapNode> SiteMapNodes { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }
    }
}