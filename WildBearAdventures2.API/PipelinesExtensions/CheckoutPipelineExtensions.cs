using Ucommerce.Extensions.Payment.Abstractions.Builder;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public static class CheckoutPipelineExtensions
    {
        public static PaymentBuilder AddMyOrderProcessingExtensions(this PaymentBuilder builder)
        {
       
            //This will insert the task 'MyCustomOrderProcessingTask' as the last task of the pipeline
            builder
                .OrderProcessingPipelines
                .GetByAlias("ToCompletedOrder")
                .InsertLast<MyCustomOrderProcessingTask>();

            return builder;
        }
    }
}
