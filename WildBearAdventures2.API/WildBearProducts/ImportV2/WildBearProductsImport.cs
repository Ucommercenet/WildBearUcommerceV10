
using Elastic.Clients.Elasticsearch.Fluent;
using Elastic.Clients.Elasticsearch;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace WildBearAdventures.API.WildBearProducts.ImportV2
{
    public class WildBearProductsImport : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WildBearProductsImport(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));


            //TODO: Check with Product why I can't get this via DI
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: Remember to register the service like this
            //builder.Services.AddHostedService<SetupDefinitions>();

            //****Task Create product definition

            //Step 1 Create product definition
            //Step 2 Check if definition already exists

            using var asyncScope = _serviceProvider.CreateAsyncScope();
            var dbContext = asyncScope.ServiceProvider.GetRequiredService<UcommerceDbContext>();

            const string ProductDefinitionName = "WildCoffee";
            const string ProductDefinitionDescription = "Definition for Coffee type products";
            const string ProductDefinitionFieldName = "OriginCountry";

            //Check if definition already exists
            //TODO Improvement: Make it works for updateing ProductDefinitions as well.

            //TODO: this is never true because ProductDefinitionName is random later, but useful for next iteration.
            var exists = dbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == ProductDefinitionName);
            if (exists) { return; }



            var WildCoffeeProductDefinition = new ProductDefinitionEntity()
            {
                Name = $"{ProductDefinitionName}{RandomLetterAndNumber()}",
                Description = ProductDefinitionDescription,
                Deleted = false
            };


            dbContext.Add(WildCoffeeProductDefinition);

            dbContext.SaveChanges();

            System.Diagnostics.Debug.WriteLine($"DEV_LOG: {WildCoffeeProductDefinition.Name} added");
        }




        private string RandomLetterAndNumber()
        {
            var random = new Random();

            // Generate a random letter (A-Z)
            char letter = (char)('A' + random.Next(0, 26));

            // Generate a random number (0-9)
            int number = random.Next(0, 10);

            // Concatenate the letter and number and return as a string
            return letter.ToString() + number.ToString();
        }


    }
}
