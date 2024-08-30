
using System.Diagnostics;
using Ucommerce.API.PipelinesExtensions.Tasks;

namespace ConsoleAppForTraceTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            var smg = GetName_WithEventSource();

            Console.WriteLine($"Hello, {smg}");
        }


        public static string GetName_WithEventSource()
        {
            var firstName = "BL";
            var lastName = "Kjaer";


            var primeNumbers = SimulateWorkload.CalculatePrimes(50000);
            var superPrimeNumbers = SimulateWorkload.CalculateSuperPrimes(primeNumbers);
            var dobbleSuperPrimes = SimulateWorkload.CalculateSuperPrimes(superPrimeNumbers);

            //Primes Numbers where the amout we have superprimed them is also a prime!! 
            var omegaSuperPrimes = SimulateWorkload.CalculateSuperPrimes(dobbleSuperPrimes);



            return $"{firstName} {lastName}, Your Omega Super Prime! number is {omegaSuperPrimes.LastOrDefault()}";
        }


        



    }
}
