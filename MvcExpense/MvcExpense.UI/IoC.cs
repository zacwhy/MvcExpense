using MvcExpense.Core;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;
using StructureMap;
using Zac.StandardCore;
using Zac.StandardCore.Services;
using Zac.StandardInfrastructure.EntityFramework;
using Zac.StandardMvc;

namespace MvcExpense.UI
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            const string nameOrConnectionString = MvcExpenseFactory.CurrentConnectionName;

            var container = GetContainer()
                .ConfigureStandardContainer( nameOrConnectionString )
                .ConfigureMvcExpenseContainer( nameOrConnectionString );

            return container;
        }

        public static IContainer GetContainer()
        {
            var container = new Container();

            container.Configure( x =>
            {
                x.Scan( scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                } );
            } );

            return container;
        }

        public static IContainer ConfigureStandardContainer( this IContainer container, string nameOrConnectionString )
        {
            container.Configure( x =>
            {
                x.For<StandardDbContext>().Use( () => new StandardDbContext( nameOrConnectionString ) );
                x.For<IStandardUnitOfWork>().Use<StandardUnitOfWork>();
                x.For<IStandardServices>().Use<StandardServices>();
            } );

            return container;
        }

        public static IContainer ConfigureMvcExpenseContainer( this IContainer container, string nameOrConnectionString )
        {
            container.Configure( x =>
            {
                x.For<MvcExpenseDbContext>().Use( () => new MvcExpenseDbContext( nameOrConnectionString ) );
                x.For<IMvcExpenseUnitOfWork>().Use<MvcExpenseUnitOfWork>();
            } );

            return container;
        }

    }
}