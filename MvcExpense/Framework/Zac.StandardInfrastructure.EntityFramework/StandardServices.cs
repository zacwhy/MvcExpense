using Zac.StandardCore;
using Zac.StandardCore.Services;
using Zac.StandardInfrastructure.EntityFramework.Services;

namespace Zac.StandardInfrastructure.EntityFramework
{
    public class StandardServices : IStandardServices
    {
        private IStandardUnitOfWork _standardUnitOfWork;

        public StandardServices( IStandardUnitOfWork standardUnitOfWork )
        {
            _standardUnitOfWork = standardUnitOfWork;
        }

        private ISiteMapNodeService _siteMapNodeService;

        public ISiteMapNodeService SiteMapNodeService
        {
            get { return _siteMapNodeService ?? ( _siteMapNodeService = new SiteMapNodeService( _standardUnitOfWork ) ); }
        }

        public void Dispose()
        {
            if ( _standardUnitOfWork != null )
            {
                _standardUnitOfWork.Dispose();
            }
        }

    }
}
