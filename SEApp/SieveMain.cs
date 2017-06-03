using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEApp
{
    public class SieveMain
    {
        //private double dResult;
        //private string strResult;
        //private SortedSet<int> m_generatedNums = new SortedSet<int>();

        public List<int> ComputePrimes( int upperBound )
        {
            //int[] primeNums = new int[upperBound+1];
            List<int> primeNums = new List<int>();
            int upperBoundSquareRoot = (int)Math.Sqrt( upperBound );

            // Perfmance improvements; convert bool array to bit array
            bool[] isNotPrimeNum = new bool[upperBound + 1];
            //System.Collections.BitArray isNotPrimeNum = new System.Collections.BitArray(upperBound+1, false);

            // We only need to go from values 2 ... sqrt(upperBound)
            // TODO: Explain why
            for( int i = 2; i <= upperBoundSquareRoot; i++ )
            {
                // If the prime number has yet to be marked TRUE
                if( !isNotPrimeNum[i] )
                {
                    // Add the initial prime number to the list
                    primeNums.Add( i );

                    // Then, mark all multiples of i*j starting from i^2
                    for( int j = i*i; j < upperBound; j += i )
                    {
                        isNotPrimeNum[j] = true;
                    }
                }
            }

            // Now that all the non prime numbers are marked
            // Iterate from sqrt(upperbound) to upperbound
            // All false/unmarked numbers are primes 
            for( int i = upperBoundSquareRoot; i <= upperBound; i++ )
            {
                if( !isNotPrimeNum[i] )
                {
                    // Add the prime numbers to the list
                    //System.Diagnostics.Debug.Write( i );
                    primeNums.Add( i );
                }
            }

            return primeNums;
            //int[] generatedNums = Enumerable.Range( 2, upperBound ).ToArray<int>();

        }

    }
}
