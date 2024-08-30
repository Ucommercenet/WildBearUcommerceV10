using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForTraceTest
{
    using System;
    using System.Collections.Generic;

    public static class FindSuperPrimeNumbers
    {
        /// <summary>
        /// Generates a list of all prime numbers up to a specified limit using the Sieve of Eratosthenes algorithm.
        /// </summary>
        /// <param name="upperLimit">The upper limit up to which primes are calculated.</param>
        /// <returns>A list of prime numbers.</returns>
        public static List<int> CalculatePrimesEratosthenesAlgorithm(int upperLimit)
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

        /// <summary>
        /// Calculates the super primes from a list of primes. A super prime is a prime number that has a prime index.
        /// </summary>
        /// <param name="primes">List of prime numbers.</param>
        /// <returns>A list of super prime numbers.</returns>
        public static List<int> CalculateSuperPrimes(List<int> primes)
        {
            List<int> superPrimes = new List<int>();

            // Iterate over the list of prime numbers
            for (int i = 0; i < primes.Count; i++)
            {
                int primePosition = i + 1; // Prime position is 1-based (i.e., the 1st prime is 2, 2nd is 3, etc.)

                // Check if the position is a prime number (super prime condition)
                if (IsPrime(primePosition))
                {
                    superPrimes.Add(primes[i]); // Add the prime number at a prime position to the super primes list
                }
            }

            return superPrimes;
        }

        /// <summary>
        /// Checks if a number is a prime.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number is prime, otherwise false.</returns>
        public static bool IsPrime(int number)
        {
            if (number <= 1) return false; // 0 and 1 are not prime numbers
            if (number <= 3) return true;  // 2 and 3 are prime numbers
            if (number % 2 == 0 || number % 3 == 0) return false; // Exclude multiples of 2 and 3

            // Check for divisors from 5 to sqrt(number)
            for (int i = 5; i * i <= number; i += 6)
            {
                if (number % i == 0 || number % (i + 2) == 0)
                    return false;
            }

            return true;
        }
    }

}
