using System.Data.Entity;
using MvcExpense.Core.Models;

namespace MvcExpense.Models
{
    public partial class MvcExpenseDbContext : DbContext
    {
        //private MvcExpenseDbContext()
        //    //: base( "name=MvcExpenseDbContext" )
        //{
        //    throw new NotSupportedException();
        //}

        // todo remove
        public static MvcExpenseDbContext GetInstance()
        {
            return new MvcExpenseDbContext( "zExpenseConnectionString" );
        }

        public MvcExpenseDbContext( string nameOrConnectionString )
            : base( nameOrConnectionString )
        {
        }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Menu>()
                .ToTable( "App_Menu" );

            //modelBuilder.Entity<Menu>().Property( x => x.Id ).HasColumnName( "Id" );
            //throw new UnintentionalCodeFirstException();
        }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<SpecialExpense> SpecialExpenses { get; set; }
        public DbSet<OrdinaryExpense> OrdinaryExpenses { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}