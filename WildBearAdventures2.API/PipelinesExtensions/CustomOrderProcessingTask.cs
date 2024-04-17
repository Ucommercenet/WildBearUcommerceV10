using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public class CustomOrderProcessingTask : IPipelineTask<OrderProcessingInput, OrderProcessingOutput>
    {
        public CascadeMode CascadeMode => CascadeMode.Continue;

        public Task Execute(PipelineTaskArgs<OrderProcessingInput, OrderProcessingOutput> subject, PipelineContext context)
        {
            if (subject.Input.Order.Customer.FirstName != "Joe")
            {
                subject.Input.Order.Customer.FirstName = "Joe";
                subject.Input.Order.BillingAddress.FirstName = "Joe";
            }

            return Task.CompletedTask;
        }
    }
}
