using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucommerce.API.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsoleAppForTraceTest
{
    public static class SimulateWorkload
    {
        public static List<int> CalculatePrimes(int upperLimit)
        {
            CustomUcommerceEventSource.Log.SimWork_Start("Calculating prime numbers");

            var primes = new List<int>();
            for (int number = 2; number <= upperLimit; number++)
            {
                if (IsPrimeOptimized(number))
                {

                   
                    primes.Add(number);
                }
            }

            CustomUcommerceEventSource.Log.SimWork_Finish("Finished calculating prime numbers");
            return primes;
        }

        public static bool IsPrimeOptimized(int number)
        {
            if (number <= 1) return false;
            if (number <= 3) return true;
            if (number % 2 == 0 || number % 3 == 0) return false;
            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }
            return true;
        }


        public static List<int> CalculatePrimesWithEratosthenes(int upperLimit)
        {
            bool[] isPrime = new bool[upperLimit + 1]; // Boolean array to mark non-prime numbers
            List<int> primes = new List<int>(); // List to store prime numbers

            // Initialize all entries as true. A value in isPrime[i] will be false if i is Not a prime, true if i is a prime.
            for (int i = 2; i <= upperLimit; i++)
            {
                isPrime[i] = true;
            }

            // Sieve of Eratosthenes algorithm
            for (int p = 2; p * p <= upperLimit; p++)
            {
                if (isPrime[p]) // If 'p' is a prime
                {
                    // Mark all multiples of 'p' as non-prime
                    for (int i = p * p; i <= upperLimit; i += p)
                    {
                        isPrime[i] = false;
                    }
                }
            }

            // Collect all primes
            for (int p = 2; p <= upperLimit; p++)
            {
                if (isPrime[p])
                {
                    primes.Add(p); // Add prime number to the list
                }
            }

            return primes;
        }


        public static List<int> CalculateSuperPrimes(List<int> primes)
        {
            List<int> superPrimesFound = new List<int>();

            // Iterate over the list of prime numbers
            for (int i = 0; i < primes.Count; i++)
            {
                int primePosition = i + 1; // Prime position is 1-based (i.e., the 1st prime is 2, 2nd is 3, etc.)                               

                // Check if the position is a prime number (super prime condition)
                if (IsPrimeOptimized(primePosition))
                {
                    var superPrime = primes[i];

                    CustomUcommerceEventSource.Log.FoundSuperPrimeNumber(superPrime, primePosition);
                    superPrimesFound.Add(superPrime); // Add the prime number at a prime position to the super primes list
                }
            }

            return superPrimesFound;
        }

    }
}
