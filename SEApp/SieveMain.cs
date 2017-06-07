using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace SEApp
{
    public class SieveMain
    {
        public List<int> ComputePrimesBasic( int maxPrime )
        {
            List<int> primeNums = new List<int>();
            bool[] markedNums = new bool[maxPrime+1];

            // Loop through all numbers starting from 2 to sqrt(n)
            // Only need to go to sqrt(n) since all numbers after sqrt(n) will have been marked already
            int maxPrimeSquareRoot = (int)Math.Floor( Math.Sqrt( maxPrime ) ) + 1;
            for( int i = 2; i < maxPrimeSquareRoot; i++ )
            {
                // If this has yet to be marked as a non-prime number
                if( !markedNums[i] )
                {
                    // Loop through all multiples of the prime number starting from the prime squared
                    // All values from the prime number to the prime number squared will have been marked already
                    for( int j = i*i; j < maxPrime; j += i )
                    {
                        // Mark all multiples of the prime since these aren't prime numbers
                        markedNums[j] = true;
                    }
                }
            }

            // Loop through all marked numbers and add the numbers to the list that are still unmarked (prime numbers)
            for( int i = 2; i < maxPrime; i++ )
            {
                // Add all unmarked numbers (prime numbers) to the return list
                if( !markedNums[i] )
                    primeNums.Add( i );
            }
            return primeNums;
        }

        public List<int> ComputePrimesSegmented( int maxPrime )
        {
            List<int> primeNums = new List<int>();
            List<int> primeNumsBefore = new List<int>();

            // Define a limit such that the numbers from 2 to maxPrime can be segmented
            // and the prime numbers can be calculated for each segment/
            // The size of each segment is the limit value
            int limit = (int)Math.Floor( Math.Sqrt( maxPrime ) ) + 1;

            // First compute all the prime numbers from 2 to sqrt(maxPrime);
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
            // the input maxPrime
            while( lowerBoundSegment < maxPrime )
            {
                // Compute the prime numbers for this segment
                List<int> primesTemp = ComputeSegment( primeNumsBefore, limit, lowerBoundSegment, upperBoundSegment, maxPrime );
                primeNums.AddRange( primesTemp );

                // Set the lower and upper bounds of the current segment to the bounds of the new segment
                lowerBoundSegment += limit;
                upperBoundSegment += limit;

                // If we've reached the last segment, the upper bound is now the input maxPrime
                if( upperBoundSegment >= maxPrime )
                    upperBoundSegment = maxPrime;
            }
            return primeNums;
        }

        public List<int> ComputePrimesSegmentedAsync( int maxPrime )
        {
            List<int> primeNums = new List<int>();
            List<int> primeNumsBefore = new List<int>();

            // Define a limit such that the numbers from 2 to maxPrime can be segmented
            // and the prime numbers can be calculated for each segment/
            // The size of each segment is the limit value
            int limit = (int)Math.Floor( Math.Sqrt( maxPrime ) ) + 1;

            // First compute all the prime numbers from 2 to sqrt(maxPrime);
            // i.e The first segment
            primeNumsBefore = ComputePrimesBasic( limit );

            // All the first segment of prime numbers to the list
            primeNums.AddRange( primeNumsBefore );

            // Define the lower and upper bounds for the next segment
            // These bounds are updated for each segment
            int upperBoundSegment = 2*limit;

            // Calculate prime numbers for each segment and stop when we get to the last segment
            // i.e. last segment is defined where the lower bound of the last segment is still less than
            // the input maxPrime
            int parallelCount = 0;
            int maxProcessors = Environment.ProcessorCount;
            Task[] tasks = new Task[maxProcessors];
            bool threadsComplete = false;
            var segmentsList = new System.Collections.Concurrent.ConcurrentBag<List<int>>();

            for( int lowerBoundSegment = limit; lowerBoundSegment < maxPrime; )
            {
                // Variable capture to ensure bounds are not changed in currently running task
                int low = lowerBoundSegment;
                int high = upperBoundSegment;

                //ComputeSegment( primeNums, limit, lowerBoundSegment, upperBoundSegment, maxPrime, ref primeNums );
                tasks[parallelCount] =  Task.Run( () =>
                    segmentsList.Add( ComputeSegment( primeNumsBefore, limit, low, high, maxPrime ) ) );

                // Only spawn max parallel processes equal to maxProcessors at a time;
                // If we're on the last segment, i.e lowerBound + limit > maxPrime, then push remaining segments
                int lastSegmentValue = lowerBoundSegment + limit;
                if( lastSegmentValue >= maxPrime || parallelCount >= maxProcessors-1 )
                {
                    // Once all tasks are complete, loop through segments list and push each set of primes into master container
                    Task.WaitAll( tasks );
                    foreach( List<int> segment in segmentsList )
                        primeNums.AddRange( segment );

                    // Clear contents in ConcurrentBag<List<int>> segmentsList;
                    List<int> emptyList;
                    while( !segmentsList.IsEmpty )
                        segmentsList.TryTake( out emptyList );

                    threadsComplete = true;
                }

                // Set the lower and upper bounds of the current segment to the bounds of the new segment
                upperBoundSegment += limit;
                lowerBoundSegment += limit;

                // If we've reached the last segment, the upper bound is now the input maxPrime
                if( upperBoundSegment >= maxPrime )
                    upperBoundSegment = maxPrime;

                // Only increment counter when there are more threads to spawn
                if( !threadsComplete )
                    parallelCount++;
                else
                {
                    parallelCount = 0;
                    threadsComplete = false;
                }

            }
            // Sort the list since the asnychronous tasks will most likely have inserted the primes out of order
            primeNums.Sort();
            return primeNums;
        }

        protected List<int> ComputeSegment( List<int> initialPrimes, int limit, int lowerBound, int upperBound, int maxPrime )
        {
            List<int> primesResult = new List<int>();
            // Create a boolean array the size of the segment + 1 to mark values that are prime numbers
            bool[] markedNums = new bool[limit+1];

            // Initialize all the values in the boolean array to true
            for( int i = 0; i < markedNums.Length; i++ )
                markedNums[i] = true;

            // Loop over all the prime numbers that have already been calculated (the first segment)
            for( int i = 0; i < initialPrimes.Count(); i++ )
            {
                // Start calculating the new primes at a lowerLimit value, 
                // where the lowerLimit is in current segment and is the lowest
                // number in the segment that is a multiple of the current prime number
                int lowerLimit = (int)Math.Floor( (double)lowerBound/initialPrimes[i] )*initialPrimes[i];
                if( lowerLimit < lowerBound )
                    lowerLimit += initialPrimes[i];

                // Loop through the segment, from the lowerLimit/first multiple to the segment's upper bound
                // and set all multiples of the current prime number to false
                for( int j = lowerLimit; j < upperBound; j += initialPrimes[i] )
                {
                    markedNums[j-lowerBound] = false;
                }
            }

            // For all values in this segment and markedNums that are true/prime numbers, add them to the return list
            for( int i = lowerBound; i < upperBound; i++ )
            {
                if( markedNums[i-lowerBound] )
                    primesResult.Add( i );
            }
            return primesResult;
        }
    }
}