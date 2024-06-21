using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace Ucommerce.API.PipelinesExtensions
{
    public class CoffeeProductDescriptionTask : IPipelineTask<CreateProductInput, CreateProductOutput>

    {
        public CascadeMode CascadeMode => CascadeMode.Stop;

        public Task Execute(PipelineContext<CreateProductInput, CreateProductOutput> context, CancellationToken cancellationToken)
        {

            //DO if name contains coffee add description
            if (context.Input.Name.Contains("Coffee"))
            {

                var DanishDescription = context.Output.Product.ProductDescriptions.FirstOrDefault(x => x.CultureCode == "da-DK");

                DanishDescription.ShortDescription = "This is a great coffee";

                //TODO: Handle if there is not a Danish description

            }

            return Task.CompletedTask;
        }


    }
}
