using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public class MyCustomOrderProcessingTask : IPipelineTask<OrderProcessingInput, OrderProcessingOutput>
    {
        public CascadeMode CascadeMode => CascadeMode.Continue;

        public Task Execute(PipelineTaskArgs<OrderProcessingInput, OrderProcessingOutput> subject, PipelineContext context)
        {

            //subject.Input.Order.


            throw new NotImplementedException();
            // Your custom logic.
        }
    }
}
