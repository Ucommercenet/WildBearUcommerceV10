using ConsoleAppForTraceTest;
using Flurl.Util;
using Microsoft.EntityFrameworkCore;
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


            //Simulate some heavy work
            Thread.Sleep(1000);            
            var primeNumbers = SimulateWorkload.CalculatePrimes(90000);

            var msg = $"This order was delayed";

            var shoppingCartAudits = context.Output.Cart.Audits;

            //var cartStatusAudit = new OrderStatusAuditEntity
            //{
            //    CartId = cart.Id,
            //    NewOrderStatusId = 1, //TODO to be removed when new basket status is removed.
            //    Message = context.Input.Message
            //};
            //await _dbContext.Set<OrderStatusAuditEntity>()
            //    .AddAsync(cartStatusAudit, cancellationToken);

            //context.Output.OrderStatusAudit = cartStatusAudit;




            CustomUcommerceEventSource.Log.Pipeline_Finish("Finished_DelayToCartPipelineTask", productName, billingAddressName);
            return Task.CompletedTask;
        }

       
    }
}

