using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Controllers;
using MvcExpense.DAL;
using MvcExpense.Models;
using NUnit.Framework;
using Rhino.Mocks;
using Zac.DateRange;

namespace MvcExpense.Tests.Controllers
{
    [TestFixture]
    public class OrdinaryExpenseControllerTester
    {
        [Test]
        public void TestMethod1()
        {
            // Arrange
            DateRange dateRange = new DateRange
            {
                StartDate = new DateTime( 2012, 5, 31 ),
                EndDate = new DateTime( 2012, 6, 1 )
            };

            var list = new List<OrdinaryExpense>();
            list.Add( new OrdinaryExpense() );
            IQueryable<OrdinaryExpense> query = list.AsQueryable();

            IOrdinaryExpenseRepository ordinaryExpenseRepository = S<IOrdinaryExpenseRepository>();
            //ordinaryExpenseRepository.Stub( x => x.GetQueryable() ).Return( query );
            ordinaryExpenseRepository.Stub( x => x.GetWithDateRange( dateRange ) ).Return( query );

            IMvcExpenseUnitOfWork unitOfWork = S<IMvcExpenseUnitOfWork>();
            unitOfWork.Expect( x => x.OrdinaryExpenseRepository ).Return( ordinaryExpenseRepository );

            // Act
            var controller = new OrdinaryExpenseController( unitOfWork );
            ViewResult viewResult = controller.Index( 2012, 5, 31 );

            // Assert
            Assert.That( viewResult.ViewName, Is.EqualTo( "a" ) );
            //Assert.That( viewResult.ViewData.Model, Is.EqualTo( "" ) );
        }

        private T S<T>( params object[] argumentsForConstructor ) where T : class
        {
            return MockRepository.GenerateStub<T>( argumentsForConstructor );
        }

    }
}
