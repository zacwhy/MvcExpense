using System.Net;
using System.Web.Mvc;
using Zac.StandardCore;
using Zac.StandardHelper;

namespace Zac.StandardMvc
{
    public abstract class AbstractStandardController : Controller
    {
        protected IStandardUnitOfWork StandardUnitOfWork { get; private set; }

        // todo remove. required for T4MVC.
        protected AbstractStandardController()
        {
            //throw new Exception( "todo remove. required for T4MVC." );
        }

        protected AbstractStandardController( IStandardUnitOfWork standardUnitOfWork )
        {
            StandardUnitOfWork = standardUnitOfWork;
        }

        protected override void Dispose( bool disposing )
        {
            if ( StandardUnitOfWork != null )
            {
                StandardUnitOfWork.Dispose();
            }

            base.Dispose( disposing );
        }

        private ErrorLogHelper _errorLogHelper;
        protected ErrorLogHelper ErrorLogHelper
        {
            get
            {
                if ( _errorLogHelper == null )
                {
                    string userId = User.Identity.Name;
                    string hostIP = Dns.GetHostName();
                    string clientIP = Request.UserHostName;

                    // todo should create new db connection
                    _errorLogHelper = new ErrorLogHelper( StandardUnitOfWork, userId, hostIP, clientIP );
                }

                return _errorLogHelper;
            }
        }

    }
}