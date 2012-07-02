// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace MvcExpense.UI.Controllers {
    public partial class OrdinaryExpenseController {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected OrdinaryExpenseController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result) {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ViewResult Index() {
            return new T4MVC_ViewResult(Area, Name, ActionNames.Index);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ViewResult Details() {
            return new T4MVC_ViewResult(Area, Name, ActionNames.Details);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult CreateAndNew() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.CreateAndNew);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Edit() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult Delete() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public System.Web.Mvc.ActionResult DeleteConfirmed() {
            return new T4MVC_ActionResult(Area, Name, ActionNames.DeleteConfirmed);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public OrdinaryExpenseController Actions { get { return MVC.OrdinaryExpense; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "OrdinaryExpense";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "OrdinaryExpense";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass {
            public readonly string Index = "Index";
            public readonly string Details = "Details";
            public readonly string Create = "Create";
            public readonly string CreateAndNew = "CreateAndNew";
            public readonly string Edit = "Edit";
            public readonly string Delete = "Delete";
            public readonly string DeleteConfirmed = "Delete";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants {
            public const string Index = "Index";
            public const string Details = "Details";
            public const string Create = "Create";
            public const string CreateAndNew = "CreateAndNew";
            public const string Edit = "Edit";
            public const string Delete = "Delete";
            public const string DeleteConfirmed = "Delete";
        }


        static readonly ActionParamsClass_Index s_params_Index = new ActionParamsClass_Index();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Index IndexParams { get { return s_params_Index; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Index {
            public readonly string year = "year";
            public readonly string month = "month";
            public readonly string day = "day";
        }
        static readonly ActionParamsClass_Details s_params_Details = new ActionParamsClass_Details();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Details DetailsParams { get { return s_params_Details; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Details {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_CreateAndNew s_params_CreateAndNew = new ActionParamsClass_CreateAndNew();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CreateAndNew CreateAndNewParams { get { return s_params_CreateAndNew; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CreateAndNew {
            public readonly string input = "input";
        }
        static readonly ActionParamsClass_Edit s_params_Edit = new ActionParamsClass_Edit();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Edit EditParams { get { return s_params_Edit; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Edit {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_Delete s_params_Delete = new ActionParamsClass_Delete();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Delete DeleteParams { get { return s_params_Delete; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Delete {
            public readonly string id = "id";
        }
        static readonly ActionParamsClass_DeleteConfirmed s_params_DeleteConfirmed = new ActionParamsClass_DeleteConfirmed();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_DeleteConfirmed DeleteConfirmedParams { get { return s_params_DeleteConfirmed; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_DeleteConfirmed {
            public readonly string id = "id";
        }
        static readonly ViewNames s_views = new ViewNames();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewNames Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewNames {
            public readonly string Create = "~/Views/OrdinaryExpense/Create.cshtml";
            public readonly string Delete = "~/Views/OrdinaryExpense/Delete.cshtml";
            public readonly string Details = "~/Views/OrdinaryExpense/Details.cshtml";
            public readonly string Edit = "~/Views/OrdinaryExpense/Edit.cshtml";
            public readonly string Index = "~/Views/OrdinaryExpense/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class T4MVC_OrdinaryExpenseController: MvcExpense.UI.Controllers.OrdinaryExpenseController {
        public T4MVC_OrdinaryExpenseController() : base(Dummy.Instance) { }

        public override System.Web.Mvc.ViewResult Index(int? year, int? month, int? day) {
            var callInfo = new T4MVC_ViewResult(Area, Name, ActionNames.Index);
            callInfo.RouteValueDictionary.Add("year", year);
            callInfo.RouteValueDictionary.Add("month", month);
            callInfo.RouteValueDictionary.Add("day", day);
            return callInfo;
        }

        public override System.Web.Mvc.ViewResult Details(long id) {
            var callInfo = new T4MVC_ViewResult(Area, Name, ActionNames.Details);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create() {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Create(MvcExpense.UI.Models.Input.CreateOrdinaryExpenseInput input) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Create);
            callInfo.RouteValueDictionary.Add("input", input);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult CreateAndNew(MvcExpense.UI.Models.Input.CreateOrdinaryExpenseInput input) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.CreateAndNew);
            callInfo.RouteValueDictionary.Add("input", input);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(long id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Edit(MvcExpense.UI.Models.Input.EditOrdinaryExpenseInput input) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Edit);
            callInfo.RouteValueDictionary.Add("input", input);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult Delete(long id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.Delete);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

        public override System.Web.Mvc.ActionResult DeleteConfirmed(long id) {
            var callInfo = new T4MVC_ActionResult(Area, Name, ActionNames.DeleteConfirmed);
            callInfo.RouteValueDictionary.Add("id", id);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591