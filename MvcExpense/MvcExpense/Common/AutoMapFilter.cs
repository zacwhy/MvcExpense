using System;
using System.Web.Mvc;
using AutoMapper;

namespace MvcExpense.Common
{
    public class AutoMapFilter : BaseActionFilter
    {
        private readonly Type _sourceType;
        private readonly Type _destType;

        public AutoMapFilter( Type sourceType, Type destType )
        {
            _sourceType = sourceType;
            _destType = destType;
        }

        public override void OnActionExecuted( ActionExecutedContext filterContext )
        {
            object model = filterContext.Controller.ViewData.Model;
            object viewModel = Mapper.Map( model, _sourceType, _destType );
            filterContext.Controller.ViewData.Model = viewModel;
        }

    }
}
