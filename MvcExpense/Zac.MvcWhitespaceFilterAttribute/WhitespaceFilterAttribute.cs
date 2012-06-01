using System.Web;
using System.Web.Mvc;

namespace Zac.MvcWhitespaceFilterAttribute
{
    /// <summary>
    /// tugberk
    /// http://stackoverflow.com/questions/855526/removing-extra-whitespace-from-generated-html-in-mvc
    /// </summary>
    public class WhitespaceFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted( ActionExecutedContext filterContext )
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            //Temp fix. I am not sure what causes this but ContentType is coming as text/html
            //if ( filterContext.HttpContext.Request.RawUrl != "/sitemap.xml" )
            //{
            if ( response.ContentType == "text/html" && response.Filter != null )
            {
                response.Filter = new WhitespaceFilterStream( response.Filter );
            }
            //}
        }

    }
}