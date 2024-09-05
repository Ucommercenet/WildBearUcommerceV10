using ConsoleAppForTraceTest;
using Ucommerce.API.Diagnostics;
using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Pipelines;
using Ucommerce.Web.WebSite.Pipelines.Cart.AddToCart;

namespace Ucommerce.API.PipelinesExtensions.Tasks
{
    public class DelayToCartPipelineTask : IPipelineTask<AddToCartInput, AddToCartOutput>

    {
        public CascadeMode CascadeMode => CascadeMode.Stop;

        public Task Execute(PipelineContext<AddToCartInput, AddToCartOutput> context, CancellationToken cancellationToken)
        {
            using (UcommerceWildBearActivitySource.Instance.StartActivity("DelayToCartPipelineTask"));


            CustomUcommerceEventSource.Log.Pipeline_Start("Start_DelayToCartPipelineTask");



            var productName = context.Output.Product.Name;
            var billingAddressName = context.Output.Cart?.BillingAddress?.FirstName;

            Thread.Sleep(1000);

            

                var primeNumbers = SimulateWorkload.CalculatePrimes(90000);


            CustomUcommerceEventSource.Log.Pipeline_Finish("Finished_DelayToCartPipelineTask", productName, billingAddressName);
            return Task.CompletedTask;
        }


    }
}

