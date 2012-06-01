using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcExpense.Controllers;
using System.Web.Mvc;

namespace MvcExpense.Tests.Controllers
{
    [TestClass]
    public class OrdinaryExpenseControllerTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new OrdinaryExpenseController();
            ViewResult viewResult = controller.Index( null, null, null );

            //Assert.t
        }
    }
}
