using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEApp
{
    public class SieveMain
    {
        public List<int> ComputePrimesBasic( int upperBound )
        {
            List<int> primeNums = new List<int>();
            bool[] markedNums = new bool[upperBound+1];
            
            // Loop through all numbers starting from 2 to sqrt(n)
            // Only need to go to sqrt(n) since all numbers after sqrt(n) will have been marked already
            int upperBoundSquareRoot = (int)Math.Floor( Math.Sqrt( upperBound ) );
            for( int i = 2; i < upperBoundSquareRoot; i++ )
            {
                // If this has yet to be marked as a non-prime number
                if( !markedNums[i] )
                {
                    // Loop through all multiples of the prime number starting from the prime squared
                    // All values from the prime number to the prime number squared will have been marked already
                    for( int j = i*i; j < upperBound; j += i )
                    {
                        // Mark all multiples of the prime since these aren't prime numbers
                        markedNums[j] = true;
                    }
                }
            }

            // Loop through all marked numbers and add the numbers to the list that are still unmarked (prime numbers)
            for( int i = 2; i < upperBound; i++ )
            {
                if( !markedNums[i] )
                    primeNums.Add( i );
            }
            return primeNums;
        }

        public List<int> ComputePrimesSegmented( int upperBound )
        {
            List<int> primeNums = new List<int>();
            List<int> primeNumsBefore = new List<int>();

            // Define a limit such that the numbers from 2 to upperBound can be segmented
            // and the prime numbers can be calculated for each segment/
            // The size of each segment is the limit value
            int limit = (int)Math.Floor( Math.Sqrt( upperBound ) ) + 1;

            // First compute all the prime numbers from 2 to sqrt(upperBound);
            // i.e The first segment
            primeNumsBefore = ComputePrimesBasic( limit );

            // All the first segment of prime numbers to the list
            primeNums.AddRange( primeNumsBefore );

            // Define the lower and upper bounds for the next segment
            // These bounds are updated for each segment
            int lowerBoundSegment = limit;
            int upperBoundSegment = 2*limit;

            // Calculate prime numbers for each segment and stop when we get to the last segment
            // i.e. last segment is defined where the lower bound of the last segment is still less than
            // the input upperBound
            while( lowerBoundSegment < upperBound )
            {
                // Create a boolean array the size of the segment + 1 to mark values that are prime numbers
                bool[] markedNums = new bool[limit+1];

                // Initialize all the values in the boolean array to true
                for( int i = 0; i < markedNums.Length; i++ )
                    markedNums[i] = true;

                // Loop over all the prime numbers that have already been calculated (the first segment)
                for( int i = 0; i < primeNumsBefore.Count(); i++ )
                {
                    // Start calculating the new primes at a lowerLimit value, 
                    // where the lowerLimit is in current segment and is the lowest
                    // number in the segment that is a multiple of the current prime number
                    int lowerLimit = (int)Math.Floor( (double)lowerBoundSegment/primeNumsBefore[i] )*primeNumsBefore[i];
                    if( lowerLimit < lowerBoundSegment )
                        lowerLimit += primeNumsBefore[i];

                    // Loop through the segment, from the lowerLimit/first multiple to the segment's upper bound
                    // and set all multiples of the current prime number to false
                    for( int j = lowerLimit; j < upperBoundSegment; j += primeNumsBefore[i] )
                    {
                        markedNums[j-lowerBoundSegment] = false;
                    }
                }

                // For all values in this segment and markedNums that are true/prime numbers, add them to the return list
                for( int i = lowerBoundSegment; i < upperBoundSegment; i++ )
                {
                    if( markedNums[i-lowerBoundSegment] )
                        primeNums.Add( i );
                }

                // Set the lower and upper bounds of the current segment to the bounds of the new segment
                lowerBoundSegment += limit;
                upperBoundSegment += limit;

                // If we've reached the last segment, the upper bound is now the input upperBound
                if( upperBoundSegment >= upperBound )
                    upperBoundSegment = upperBound;
            }
            return primeNums;
        }

    }
}
