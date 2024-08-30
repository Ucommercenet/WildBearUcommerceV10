using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppForTraceTest
{

    [EventSource(Name = "ConsoleEventSource")]
    internal class ConsoleEventSource : EventSource
    {
        public static ConsoleEventSource Log { get; } = new ConsoleEventSource();

        [Event(1)]
        public void Start(string message)
        {
            WriteEvent(1, message);
        }

        [Event(2)]
        public void Finished(string message)
        {
            WriteEvent(2, message);
        }

        [Event(100)]
        public void FoundSuperPrimeNumber(int superPrimeNumber, int position)
        {
            WriteEvent(100, superPrimeNumber);
        }


    }
}
