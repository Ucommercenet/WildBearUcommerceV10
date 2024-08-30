using System.Diagnostics;

namespace Ucommerce.API.PipelinesExtensions.Tasks
{
    static class MyConsoleActivitySource
    {
        public static ActivitySource instance { get; } = new ActivitySource(
            "ConsoleAppForTraceTest",
            "1.0.0"
        );
    }


}