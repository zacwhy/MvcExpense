using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using Zac.MvcWhitespaceFilterAttribute;

namespace MvcExpense
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static string DefaultController
        {
            get
            {
                const string defaultValue = "Home";
                string value = ConfigurationManager.AppSettings["DefaultController"];
                if ( string.IsNullOrEmpty( value ) )
                {
                    return defaultValue;
                }
                return value;
            }
        }

        private static string DefaultAction
        {
            get
            {
                const string defaultValue = "Index";
                string value = ConfigurationManager.AppSettings["DefaultAction"];
                if ( string.IsNullOrEmpty( value ) )
                {
                    return defaultValue;
                }
                return value;
            }
        }

        public static void RegisterGlobalFilters( GlobalFilterCollection filters )
        {
            filters.Add( new HandleErrorAttribute() );
            filters.Add( new WhitespaceFilterAttribute() );
        }

        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = DefaultController, action = DefaultAction, id = UrlParameter.Optional } // Parameter defaults
                //new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
           );

            routes.MapRoute(
                "Ordinary Expense Index",
                "OrdinaryExpense/Index/Range/{year}/{month}/{day}",
                new
                {
                    controller = "OrdinaryExpense",
                    action = "Index",
                    year = UrlParameter.Optional,
                    month = UrlParameter.Optional,
                    day = UrlParameter.Optional
                }/*,
                new
                {
                    year = @"\d{4}"
                }*/
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters( GlobalFilters.Filters );
            RegisterRoutes( RouteTable.Routes );

            CreateMaps();
        }

        protected void Application_AuthenticateRequest( object sender, System.EventArgs e )
        {
            string url = HttpContext.Current.Request.FilePath;

            if ( url.EndsWith( "ext.axd" ) )
            {
                HttpContext.Current.SkipAuthorization = true;
            }
        }

        private void CreateMaps()
        {
            CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseViewModel>();
            CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseCreateModel>();
            CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseEditModel>();
            CreateDuplexMap<PaymentMethod, PaymentMethodEditModel>();
        }

        private static void CreateDuplexMap<TSource, TDestination>()
        {
            Mapper.CreateMap<TSource, TDestination>();
            Mapper.CreateMap<TDestination, TSource>();
        }

    }
}