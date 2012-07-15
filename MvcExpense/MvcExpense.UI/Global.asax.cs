using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcExpense.UI.DependencyResolution;
using Zac.MvcWhitespaceFilterAttribute;
using Zac.StandardMvc;

namespace MvcExpense.UI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new HandleErrorAttribute() );
            filters.Add( new WhitespaceFilterAttribute() );
        }

        protected void Application_Start()
        {
            ControllerBuilder.Current.SetControllerFactory( new StandardControllerFactory( IoC.Container ) );

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters( GlobalFilters.Filters );

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