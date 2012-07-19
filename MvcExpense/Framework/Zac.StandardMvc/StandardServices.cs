using Zac.StandardCore;
using Zac.StandardCore.Services;
using Zac.StandardMvc.Services;

namespace Zac.StandardMvc
{
    public class StandardServices : IStandardServices
    {
        private IStandardUnitOfWork _standardUnitOfWork;

        public StandardServices( IStandardUnitOfWork standardUnitOfWork )
        {
            _standardUnitOfWork = standardUnitOfWork;
        }

        private IErrorLogService _errorLogService;

        public IErrorLogService ErrorLogService
        {
            get { return _errorLogService ?? ( _errorLogService = new ErrorLogService( _standardUnitOfWork ) ); }
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
