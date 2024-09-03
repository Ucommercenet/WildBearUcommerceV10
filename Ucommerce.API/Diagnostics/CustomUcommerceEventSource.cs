using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucommerce.Web.Infrastructure.Persistence.Entities;

namespace Ucommerce.API.Diagnostics
{

    [EventSource(Name = "CustomUcommerceEventSource")]
    internal class CustomUcommerceEventSource : EventSource
    {
        public static CustomUcommerceEventSource Log { get; } = new CustomUcommerceEventSource();

        [Event(1)]
        public void Pipeline_Start(string message)
        {
            WriteEvent(1, message);
        }

        [Event(2)]
        public void Pipeline_Finish(string message, string productName , string cartInfo)
        {
            WriteEvent(2, message, productName, cartInfo);
        }

        [Event(3)]
        public void SimWork_Start(string message)
        {
            WriteEvent(3, message);
        }

        [Event(4)]
        public void SimWork_Finish(string message)
        {
            WriteEvent(4, message);
        }


        [Event(100)]
        public void FoundSuperPrimeNumber(int superPrimeNumber, int position)
        {
            WriteEvent(100, superPrimeNumber);
        }


    }
}
