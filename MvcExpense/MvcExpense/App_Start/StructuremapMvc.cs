using System.Web.Mvc;
using MvcExpense.UI.DependencyResolution;
using StructureMap;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcExpense.UI.App_Start.StructuremapMvc), "Start")]

namespace MvcExpense.UI.App_Start
{
    public static class StructuremapMvc
    {
        public static void Start()
        {
            IContainer container = IoC.Initialize();
            DependencyResolver.SetResolver( new StructureMapDependencyResolver( container ) );
        }
    }
}