using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public class MyCustomOrderProcessingTask : IPipelineTask<OrderProcessingInput, OrderProcessingOutput>
    {
        public CascadeMode CascadeMode => CascadeMode.Continue;

        public Task Execute(PipelineTaskArgs<OrderProcessingInput, OrderProcessingOutput> subject, PipelineContext context)
        {
            Console.WriteLine("Export Order to other system");

            if ( subject.Input.Order.Customer.FirstName != "Joe")
            {

                subject.Input.Order.Customer.FirstName = "Joe";

                subject.Input.Order.BillingAddress.FirstName = "Joe";

            }

           



            return Task.CompletedTask;
        }
    }
}
