
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEApp;


namespace SEAppUnitTest
{
    [TestClass]
    public class SieveMainTest
    {
        [TestMethod]
        public void TestComputePrimes()
        {
            // Prep test
            double upperBoundTestValue = 1000;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimes( upperBoundTestValue );
        }
    }
}
