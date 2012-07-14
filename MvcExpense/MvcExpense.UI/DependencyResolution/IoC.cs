using MvcExpense.Core;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;
using StructureMap;
using Zac.StandardCore;
using Zac.StandardInfrastructure.EntityFramework;

namespace MvcExpense.UI.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize( x =>
            {
                x.Scan( scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                } );

                const string nameOrConnectionString = MvcExpenseFactory.CurrentConnectionName;

                x.For<IStandardUnitOfWork>().Use<StandardUnitOfWork>();
                x.For<StandardDbContext>().Use( () => new StandardDbContext( nameOrConnectionString ) );

                x.For<MvcExpenseDbContext>().Use( () => new MvcExpenseDbContext( nameOrConnectionString ) );
                x.For<IMvcExpenseUnitOfWork>().Use<MvcExpenseUnitOfWork>();
            } );

            return ObjectFactory.Container;
        }
    }
}