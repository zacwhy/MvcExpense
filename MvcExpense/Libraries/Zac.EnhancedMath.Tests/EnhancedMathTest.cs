using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zac.EnhancedMath.Tests
{
    [TestClass]
    public class EnhancedMathTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //double averagePrice = EnhancedMath.RoundDown( price / consumerCount, 2 );

            double doublePrice = 6.6;
            double doubleResult = doublePrice / 3;

            decimal decimalPrice = new decimal( 6.6 );
            decimal decimalResult = decimalPrice / 3;
        }
    }
}
