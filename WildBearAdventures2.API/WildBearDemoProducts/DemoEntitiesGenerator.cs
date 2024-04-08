using System.Collections.Generic;
using System.Collections.Immutable;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;

namespace WildBearAdventures.API.WildBearDemoProducts
{
    public class DemoEntitiesGenerator
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private IEnumerable<IIndexer<ProductEntity>> _productIndexer;

        public DemoEntitiesGenerator(UcommerceDbContext ucommerceDbContext, IEnumerable<IIndexer<ProductEntity>> productIndexer)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productIndexer = productIndexer;
        }

        /// <summary>
        /// Will both create and Index the newly created products.
        /// </summary>

        public async Task<List<ProductEntity>> CreateCoffeeProducts(CancellationToken cancellationToken)
        {
            var wildCoffeeDefinition = _ucommerceDbContext.Set<ProductDefinitionEntity>().FirstOrDefault(x => x.Name == "WildCoffee");
            if (wildCoffeeDefinition == null) { throw new Exception("WildCoffee ProductDefinitionEntity not found"); }

            var demoProducts = new List<ProductEntity>
            {
                //TODO: Add option to create any number if needed
               await CreateRegularProduct(name: $"DemoCoffee{RandomLetterAndNumber()}", sku:RandomLetterAndNumber(), productDefinition: wildCoffeeDefinition, culture: "da-DK"),
            };


            var indexer = 


            await _productIndexer.Index(demoProducts.ToImmutableList(), cancellationToken);

            return demoProducts;
        }


        /// <summary>
        /// Creates a RegularProduct(aka non variant product)
        /// Note: name, DisplayName definition, culture are required        
        /// </summary>
        public async Task<ProductEntity> CreateRegularProduct(string name, string sku, ProductDefinitionEntity productDefinition, string culture, string? shortDescription = null, decimal? price = null)
        {
            var startingPrice = new PriceEntity() { };
            var priceCollection = new List<PriceEntity>  
            {
                startingPrice
            };

            var product = new ProductEntity()
            {
                Name = name,
                Sku = sku,                
                Definition = productDefinition,
                DisplayOnSite = true,
                ProductDescriptions = new List<ProductDescriptionEntity> { new ProductDescriptionEntity() { DisplayName = name, ShortDescription = shortDescription, CultureCode = culture } }
            };
            return product;
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
