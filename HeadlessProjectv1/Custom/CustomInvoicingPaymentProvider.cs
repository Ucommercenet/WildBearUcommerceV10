using System.Globalization;
using Ucommerce.Extensions.Payment.Abstractions;
using Ucommerce.Extensions.Payment.Abstractions.Exceptions;
using Ucommerce.Extensions.Payment.Abstractions.Models;
using Ucommerce.Web.Common.Extensions;
using Ucommerce.Web.Core.Pipelines.Checkout;
using Ucommerce.Web.Core.Pipelines.Order.GetOrderNumber;
using Ucommerce.Web.Infrastructure.Core;
using Ucommerce.Web.Infrastructure.Core.Models;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Extensions;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace WildBearAdventures.API.Custom
{
   public class CustomInvoicingPaymentProvider : RedirectionPaymentProvider<InvoicingCallbackRequest>
    {
        /// <inheritdoc />
        public override string Alias => "Default Payment Method Service";

        /// <inheritdoc />
        public CustomInvoicingPaymentProvider(
            IPipeline<GetOrderNumberInput, GetOrderNumberOutput> orderNumberPipeline, ICheckoutPipelineFactory checkoutPipelineFactory, UcommerceDbContext dbContext) : base(orderNumberPipeline, checkoutPipelineFactory, dbContext) { }

        /// <inheritdoc />
        public override Task<PaymentEntity> Cancel(PaymentEntity payment, CancellationToken token)
        {
            return payment.InTask();
        }

        /// <inheritdoc />
        public override Task<PaymentEntity> Capture(PaymentEntity payment, CancellationToken token)
        {
            return payment.InTask();
        }

        /// <inheritdoc />
        public override async Task<string> GetRedirectUrl(PaymentEntity payment, CultureInfo culture, CancellationToken token)
        {
            var activeProperties = payment.PaymentMethod.Properties.Where(p => p.DefinitionField != null
                && p.DefinitionField.Definition != null
                && p.DefinitionField.Definition.Guid == payment.PaymentMethod.Definition.Guid);
            if (!activeProperties.TryGetValue("AcceptUrl", out string? acceptUrl))
            {
                throw new PaymentMethodConfigurationException("AcceptUrl is not configured for payment method.");
            }

            acceptUrl = !acceptUrl.IsNullOrWhiteSpace()
                ? acceptUrl
                : activeProperties.FirstOrDefault(x => !x.DefinitionField!.Deleted && x.DefinitionField.Name == "AcceptUrl")!.DefinitionField!.DefaultValue;
            if (acceptUrl is null)
            {
                throw new PaymentMethodConfigurationException("AcceptUrl has no value or default value configured");
            }

            await SetPaymentStatus(payment, PaymentStatusCode.Captured, string.Empty, token);
            await ExecuteCheckoutPipeline(payment, token);
            return acceptUrl;
        }

        /// <inheritdoc />
        public override Task<CallbackResult> ProcessCallback(InvoicingCallbackRequest settings, CancellationToken token)
        {
            return CallbackResult.Empty.InTask();
        }

        /// <inheritdoc />
        public override Task<PaymentEntity> Refund(PaymentEntity payment, CancellationToken token)
        {
            return payment.InTask();
        }
    }
}
