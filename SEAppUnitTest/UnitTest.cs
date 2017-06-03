
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
            int upperBoundTestValue = 100;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimes( upperBoundTestValue );
        }

        [TestMethod]
        public void TestComputePrimesLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 2000000000;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimes( upperBoundTestValue );
        }


    }
}
