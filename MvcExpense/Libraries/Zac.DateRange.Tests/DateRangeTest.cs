using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zac.DateRange.Tests
{
    [TestClass]
    public class DateRangeTest
    {
        [TestMethod]
        public void TestCreateDateRangeWithYearMonthDay()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( 2012, 3, 23, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 3, 23 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 3, 24 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithYearMonth()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( 2012, 3, null, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 3, 1 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 4, 1 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithYear()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( 2012, null, null, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 1, 1 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2013, 1, 1 ) );
        }

        [TestMethod]
        public void TestCreateDateRange()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( null, null, null, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 5, 1 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 6, 1 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithYearDay()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( 2012, null, 23, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 5, 23 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 5, 24 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithMonthDay()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( null, 3, 23, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 3, 23 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 3, 24 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithMonth()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( null, 3, null, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 3, 1 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 4, 1 ) );
        }

        [TestMethod]
        public void TestCreateDateRangeWithDay()
        {
            // Arrange
            var current = new DateTime( 2012, 5, 30 );

            // Act
            DateRange dateRange = DateRange.CreateDateRange( null, null, 23, current );

            // Assert
            Assert.AreEqual( dateRange.StartDate, new DateTime( 2012, 5, 23 ) );
            Assert.AreEqual( dateRange.EndDate, new DateTime( 2012, 5, 24 ) );
        }

    }
}
