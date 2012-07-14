using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zac.StandardCore;
using Zac.StandardCore.Models;
using Zac.StandardCore.Repositories;

namespace Zac.StandardMvc.Controllers
{
    public partial class ErrorLogController : AbstractStandardController
    {
        private IErrorLogRepository Repository
        {
            get { return StandardUnitOfWork.ErrorLogRepository; }
        }

        public ErrorLogController( IStandardUnitOfWork unitOfWork )
            : base( unitOfWork )
        {
        }

        public virtual ActionResult Index()
        {
            IEnumerable<ErrorLog> entities = Repository.GetAll();
            return View();
        }

        public virtual ActionResult Details( long id )
        {
            ErrorLogHelper.Log( new Exception( "test" ) );
            return View();
        }

    }
}
