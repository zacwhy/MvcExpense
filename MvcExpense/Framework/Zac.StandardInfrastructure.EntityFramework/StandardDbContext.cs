using System.Data.Entity;
using Zac.DesignPattern.EntityFramework;
using Zac.StandardCore.Models;

namespace Zac.StandardInfrastructure.EntityFramework
{
    public class StandardDbContext : EnhancedDbContext
    {
        public StandardDbContext( string nameOrConnectionString )
            : base( nameOrConnectionString )
        {
        }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Entity<ErrorLog>().ToTable( "App_ErrorLog" );
            modelBuilder.Entity<SiteMapNode>().ToTable( "App_SiteMapNode" );
            modelBuilder.Entity<SystemParameter>().ToTable( "App_SystemParameter" );
            base.OnModelCreating( modelBuilder );
        }

        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<SiteMapNode> SiteMapNodes { get; set; }
        public DbSet<SystemParameter> SystemParameters { get; set; }
    }
}