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

            //TODO make execute asycn
            //await Task.Delay(3000);

            Thread.Sleep(3000);


            return Task.CompletedTask;
        }


    }
}

