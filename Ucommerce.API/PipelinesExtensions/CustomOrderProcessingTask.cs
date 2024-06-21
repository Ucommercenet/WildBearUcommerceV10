using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace Ucommerce.API.PipelinesExtensions
{
    public class CustomOrderProcessingTask : IPipelineTask<OrderProcessingInput, OrderProcessingOutput>
    {
        public CascadeMode CascadeMode => CascadeMode.Continue;


        public Task Execute(PipelineContext<OrderProcessingInput, OrderProcessingOutput> context, CancellationToken cancellationToken)
        {
            if (context.Input.Order.Customer.FirstName != "Joe")
            {
                context.Input.Order.Customer.FirstName = "Joe";
                context.Input.Order.BillingAddress.FirstName = "Joe";
            }

            return Task.CompletedTask;
        }


    }
}
