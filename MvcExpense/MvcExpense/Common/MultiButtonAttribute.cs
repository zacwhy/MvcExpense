using System;
using System.Reflection;
using System.Web.Mvc;

namespace MvcExpense.Common
{
    [AttributeUsage( AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
    public class MultiButtonAttribute : ActionNameSelectorAttribute
    {
        public string MatchFormKey { get; set; }
        public string MatchFormValue { get; set; }

        public override bool IsValidName( ControllerContext controllerContext, string actionName, MethodInfo methodInfo )
        {
            string value = controllerContext.HttpContext.Request[MatchFormKey];
            bool result = value != null && value == MatchFormValue;
            return result;
        }
    }
}
