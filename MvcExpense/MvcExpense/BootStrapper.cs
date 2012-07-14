using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AutoMapper;
using FluentSecurity;
using MvcExpense.Core.Models;
using MvcExpense.MvcExpenseHelper;
using MvcExpense.UI.Controllers;
using MvcExpense.UI.Models.Display;
using MvcExpense.UI.Models.Input;
using MvcExpense.ViewModels;

namespace MvcExpense.UI
{
    public static class BootStrapper
    {
        public static void RegisterRoutes( RouteCollection routes )
        {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                {
                    controller = SystemParameters.DefaultController,
                    action = SystemParameters.DefaultAction,
                    id = UrlParameter.Optional
                } // Parameter defaults
                //new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                //,new string[] { "MvcExpense.UI.Controllers" }
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

        public static void CreateAutoMapperMaps()
        {
            Zac.StandardMvc.BootStrapper.CreateAutoMapperMaps();
            //SiteMapNodeController.CreateMaps();

            Mapper.CreateMap<CreateOrdinaryExpenseInput, OrdinaryExpense>();
            Mapper.CreateMap<CreateOrdinaryExpenseInput, CreateOrdinaryExpenseDisplay>();

            Mapper.CreateMap<OrdinaryExpense, EditOrdinaryExpenseDisplay>();
            Mapper.CreateMap<EditOrdinaryExpenseInput, OrdinaryExpense>();
            Mapper.CreateMap<EditOrdinaryExpenseInput, EditOrdinaryExpenseDisplay>();

            CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseViewModel>();
            //CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseCreateModel>();
            //CreateDuplexMap<OrdinaryExpense, OrdinaryExpenseEditModel>();
            CreateDuplexMap<PaymentMethod, PaymentMethodEditModel>();
        }

        private static void CreateDuplexMap<TSource, TDestination>()
        {
            Mapper.CreateMap<TSource, TDestination>();
            Mapper.CreateMap<TDestination, TSource>();
        }

        public static void ConfigureSecurity()
        {
            SecurityConfigurator.Configure( configuration =>
            {
                // Let Fluent Security know how to get the authentication status of the current user
                configuration.GetAuthenticationStatusFrom( () => HttpContext.Current.User.Identity.IsAuthenticated );

                //string username = HttpContext.Current.User.Identity.Name;

                // Let Fluent Security know how to get the roles for the current user
                //configuration.GetRolesFrom( () => GetCurrentUserRoles() );
                configuration.GetRolesFrom( () => MySecurityHelper.GetCurrentUserRoles() );

                // This is where you set up the policies you want Fluent Security to enforce on your controllers and actions
                configuration.For<HomeController>().Ignore();
                configuration.For<AccountController>().DenyAuthenticatedAccess();
                configuration.For<AccountController>( x => x.ChangePassword() ).DenyAnonymousAccess();
                configuration.For<AccountController>( x => x.LogOff() ).DenyAnonymousAccess();

                configuration.For<OrdinaryExpenseController>( x => x.Index( null, null, null ) ).DenyAnonymousAccess();
                configuration.For<OrdinaryExpenseController>( x => x.Create() ).RequireRole( "user" );
            } );

            GlobalFilters.Filters.Add( new HandleSecurityAttribute(), 0 );
        }
    }

    public static class MySecurityHelper
    {
        public static IEnumerable<object> GetCurrentUserRoles()
        {
            string username = HttpContext.Current.User.Identity.Name;
            string[] roles = Roles.GetRolesForUser( username );
            return roles;
        }
    }
}