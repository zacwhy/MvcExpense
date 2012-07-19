using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Zac.StandardCore.Models;
using Zac.StandardCore.Services;

namespace Zac.StandardMvc.Controllers
{
    public partial class ErrorLogController : AbstractStandardController
    {
        public ErrorLogController( IStandardServices standardServices )
            : base( standardServices )
        {
        }

        public virtual ActionResult Index()
        {
            IEnumerable<ErrorLog> entities = StandardServices.ErrorLogService.FindAll();
            return View();
        }

        public virtual ActionResult Details( long id )
        {
            ErrorLogHelper.Log( new Exception( "test" ) );
            return View();
        }

    }
}
