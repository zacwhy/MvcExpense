using MvcExpense.Core;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;
using StructureMap;
using Zac.StandardCore;
using Zac.StandardInfrastructure.EntityFramework;

namespace MvcExpense.UI
{
    public static class IoC
    {
        private static IContainer _container;

        public static IContainer Container
        {
            get { return _container ?? ( _container = Initialize() ); }
        }

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

                x.For<StandardDbContext>().Use( () => new StandardDbContext( nameOrConnectionString ) );
                x.For<IStandardUnitOfWork>().Use<StandardUnitOfWork>();

                x.For<MvcExpenseDbContext>().Use( () => new MvcExpenseDbContext( nameOrConnectionString ) );
                x.For<IMvcExpenseUnitOfWork>().Use<MvcExpenseUnitOfWork>();
            } );

            return ObjectFactory.Container;
        }
    }
}