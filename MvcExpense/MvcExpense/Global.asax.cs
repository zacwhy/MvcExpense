using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMapper;
using MvcExpense.Models;
using MvcExpense.ViewModels;

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
        }

        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = DefaultController, action = DefaultAction, id = UrlParameter.Optional } // Parameter defaults
            );

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            //);

            routes.MapRoute(
                "OrdinaryExpense-ByMonth",
                "OrdinaryExpense/{action}/{year}/{month}",
                new
                {
                    controller = "OrdinaryExpense",
                    action = "Index",
                    //year = UrlParameter.Optional,
                    month = UrlParameter.Optional
                }// Parameter defaults
            );

            routes.MapRoute(
                "OrdinaryExpense-ByYear",
                "OrdinaryExpense/{action}/{year}",
                new
                {
                    controller = "OrdinaryExpense",
                    action = "Index",
                    year = UrlParameter.Optional
                }// Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters( GlobalFilters.Filters );
            RegisterRoutes( RouteTable.Routes );

            CreateMaps();
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