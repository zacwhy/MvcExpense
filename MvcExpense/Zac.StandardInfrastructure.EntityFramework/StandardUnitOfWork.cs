using Zac.DesignPattern.EntityFramework.UnitOfWork;
using Zac.StandardCore;
using Zac.StandardCore.Repositories;
using Zac.StandardInfrastructure.EntityFramework.Repositories;

namespace Zac.StandardInfrastructure.EntityFramework
{
    public class StandardUnitOfWork : UnitOfWorkBase, IStandardUnitOfWork
    {
        public StandardUnitOfWork( StandardDbContext context )
            : base( context )
        {
        }

        private ISiteMapNodeRepository _siteMapNodeRepository;
        public ISiteMapNodeRepository SiteMapNodeRepository
        {
            get { return _siteMapNodeRepository ?? ( _siteMapNodeRepository = new SiteMapNodeRepository( Context ) ); }
        }

        private ISystemParameterRepository _systemParameterRepository;
        public ISystemParameterRepository SystemParameterRepository
        {
            get { return _systemParameterRepository ?? ( _systemParameterRepository = new SystemParameterRepository( Context ) ); }
        }

    }
}