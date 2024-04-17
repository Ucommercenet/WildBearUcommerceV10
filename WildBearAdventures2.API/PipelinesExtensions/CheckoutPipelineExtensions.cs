using Ucommerce.Extensions.Payment.Abstractions.Builder;
using Ucommerce.Web.BackOffice.Pipelines.Product;
using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.Infrastructure.DependencyInjection;
using Ucommerce.Web.Infrastructure.Pipelines;
using Ucommerce.Web.WebSite.Pipelines.Cart.CalculateCart;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public static class CheckoutPipelineExtensions
    {
        public static PaymentBuilder AddCustomOrderProcessingTask(this PaymentBuilder builder)
        {

            //This will insert the task 'CustomOrderProcessingTask' as the last task of the pipeline
            builder
                .OrderProcessingPipelines
                .GetByAlias("ToCompletedOrder")
                .InsertLast<CustomOrderProcessingTask>();

            return builder;
        }

        public static IUcommerceBuilder AddCoffeeProductDescriptionTask(this IUcommerceBuilder builder)
        {
            //This will insert the task 'CustomOrderProcessingTask' before CreateProductIndexingPipelineTask
            builder.InsertPipelineTaskBefore<IPipelineTask<CreateProductInput, CreateProductOutput>, CoffeeProductDescriptionTask>
                (before: typeof(CreateProductIndexingPipelineTask));

            return builder;
        }



    }
}
