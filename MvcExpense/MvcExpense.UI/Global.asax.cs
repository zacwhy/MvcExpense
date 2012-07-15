using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Zac.StandardMvc;

namespace MvcExpense.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            StandardIoC.Container = IoC.Container;
            ControllerBuilder.Current.SetControllerFactory( new StandardControllerFactory( IoC.Container ) );

            AreaRegistration.RegisterAllAreas();

            BootStrapper.RegisterGlobalFilters( GlobalFilters.Filters );
            BootStrapper.RegisterRoutes( RouteTable.Routes );
            BootStrapper.CreateAutoMapperMaps();
            //BootStrapper.ConfigureSecurity();
        }

        protected void Application_AuthenticateRequest( object sender, System.EventArgs e )
        {
            //string url = HttpContext.Current.Request.FilePath;

            //if ( url.EndsWith( "ext.axd" ) )
            //{
            //    HttpContext.Current.SkipAuthorization = true;
            //}
        }

    }

}