using System.Collections.Generic;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;

namespace WildBearAdventures.API.WildBearDemoProducts
{
    public class DemoEntitiesGenerator
    {
        private readonly UcommerceDbContext _ucommerceDbContext;

        public DemoEntitiesGenerator(UcommerceDbContext ucommerceDbContext)
        {
            _ucommerceDbContext = ucommerceDbContext;
        }

        public List<ProductEntity> CreateCoffeeProducts()
        {
            var wildCoffeeDefinition = _ucommerceDbContext.Set<ProductDefinitionEntity>().FirstOrDefault(x => x.Name == "WildCoffee");
            if (wildCoffeeDefinition == null) { throw new Exception("WildCoffee ProductDefinitionEntity not found"); }

            var demoProducts = new List<ProductEntity>
            {
                //TODO: Add option to create any number if needed
                CreateRegularProduct(name: $"DemoCoffee{RandomLetterAndNumber()}", sku:RandomLetterAndNumber(), productDefinition: wildCoffeeDefinition, culture: "da-DK"),
                CreateRegularProduct(name: $"DemoCoffee{RandomLetterAndNumber()}", sku:RandomLetterAndNumber(), productDefinition: wildCoffeeDefinition, culture: "da-DK")
            };
            return demoProducts;
        }


        /// <summary>
        /// Creates a RegularProduct(aka non variant product)
        /// Note: name, DisplayName definition, culture are required        
        /// </summary>
        public ProductEntity CreateRegularProduct(string name, string sku, ProductDefinitionEntity productDefinition, string culture, string? shortDescription = null, decimal? price = null)
        {
            var product = new ProductEntity()
            {
                Name = name,
                Sku = sku,
                Definition = productDefinition,
                
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
