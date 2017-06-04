
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEApp;


namespace SEAppUnitTest
{
    // TODO: insert try/catch blocks for all tests
    // for better descriptive error handling
    [TestClass]
    public class SieveMainTest
    {
        [TestMethod]
        public void TestComputePrimesBasic()
        {
            // Prep test
            int upperBoundTestValue = 100;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimesBasic( upperBoundTestValue );
        }

        [TestMethod]
        public void TestComputePrimesSegmented()
        {
            // Prep test
            int upperBoundTestValue = 100;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimesSegmented( upperBoundTestValue );
        }

        [TestMethod]
        public void TestComputePrimesBasicLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 1000000000;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimesBasic( upperBoundTestValue );
        }

        [TestMethod]
        public void TestComputePrimesSegmentedLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 1000000000;
            SieveMain sieve = new SEApp.SieveMain();

            // Execute test
            sieve.ComputePrimesSegmented( upperBoundTestValue );
        }
    }
}
