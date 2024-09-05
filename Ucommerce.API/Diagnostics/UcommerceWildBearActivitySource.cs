using System.Diagnostics;

namespace Ucommerce.API.PipelinesExtensions.Tasks
{
    static class UcommerceWildBearActivitySource
    {
        public static ActivitySource Instance { get; } = new ActivitySource(
            "UcommerceWildBearActivitySource",
            "1.0.0"
        );
    }


}