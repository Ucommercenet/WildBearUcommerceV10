using Ucommerce.Web.BackOffice.Pipelines.Product.CreateProduct;
using Ucommerce.Web.Core.Pipelines.OrderProcessing;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace WildBearAdventures.API.PipelinesExtensions
{
    public class CoffeeProductDescriptionTask : IPipelineTask<CreateProductInput, CreateProductOutput>
    {
        public CascadeMode CascadeMode => CascadeMode.Stop;

        public Task Execute(PipelineTaskArgs<CreateProductInput, CreateProductOutput> subject, PipelineContext context)
        {
            //DO if name contains coffee add description
            if (subject.Input.Name.Contains("Coffee"))
            {
                var DanishDescription = subject.Output.Product.ProductDescriptions.FirstOrDefault(x => x.CultureCode == "da-DK");

                DanishDescription.ShortDescription = "This is a great coffee";

                //TODO: Add more languages and handle if there is not a Danish description

            }

            return Task.CompletedTask;
        }
    }
}
