
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SEApp;
using System.Collections.Generic;


namespace SEAppUnitTest
{
    // TODO: insert try/catch blocks for all tests
    // for better descriptive error handling
    [TestClass]
    public class SieveMainTests
    {
        [TestMethod]
        public void TestComputePrimesBasic()
        {
            // Prep test
            int upperBoundTestValue = 100;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 25;

            // Execute test
            primeNums = sieve.ComputePrimesBasic( upperBoundTestValue );

            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }

        [TestMethod]
        public void TestComputePrimesSegmented()
        {
            // Prep test
            int upperBoundTestValue = 100;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 25;


            // Execute test
            primeNums = sieve.ComputePrimesSegmented( upperBoundTestValue );
            
            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }

        [TestMethod]
        public void TestComputePrimesSegmentedAsync()
        {
            // Prep test
            int upperBoundTestValue = 100;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 25;

            // Execute test
            primeNums = sieve.ComputePrimesSegmentedAsync( upperBoundTestValue );

            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }

        [TestMethod]
        public void TestComputePrimesBasicLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 1000000000;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 50847534;

            // Execute test
            primeNums = sieve.ComputePrimesBasic( upperBoundTestValue );

            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }

        [TestMethod]
        public void TestComputePrimesSegmentedLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 1000000000;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 50847534;

            // Execute test
            primeNums = sieve.ComputePrimesSegmented( upperBoundTestValue );

            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }

        [TestMethod]
        public void TestComputePrimesSegmentedAsyncLargeInt()
        {
            // Prep test
            int upperBoundTestValue = 1000000000;
            SieveMain sieve = new SieveMain();
            List<int> primeNums = new List<int>();
            int expectedPrimeNumCount = 50847534;

            // Execute test
            primeNums = sieve.ComputePrimesSegmentedAsync( upperBoundTestValue );

            //Assert
            Assert.AreEqual( expectedPrimeNumCount, primeNums.Count );
        }
    }
}
