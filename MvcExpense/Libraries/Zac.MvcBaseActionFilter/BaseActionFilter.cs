using System;
using System.Web.Mvc;

namespace Zac.MvcBaseActionFilter
{
    public class BaseActionFilter : IActionFilter
    {
        public virtual void OnActionExecuted( ActionExecutedContext filterContext )
        {
            throw new NotImplementedException();
        }

        public virtual void OnActionExecuting( ActionExecutingContext filterContext )
        {
            throw new NotImplementedException();
        }
    }
}
