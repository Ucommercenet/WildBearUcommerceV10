using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;


namespace WildBearAdventures.API.WildBearProducts
{
    public class SetupDefinitions : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SetupDefinitions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await using var asyncScope = _serviceProvider.CreateAsyncScope();
            var dbContext = asyncScope.ServiceProvider.GetRequiredService<UcommerceDbContext>();

            // Set up data using dbContext

            //var exists = dbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == "Coffee");
            //if (exists) { return; }

            var shortTextDataType = dbContext.Set<DataTypeEntity>().First(x => x.DefinitionName == "ShortText");

            //Add Coffee Product Definition

            var coffeeDefinition = CreateCoffeeProductDefinition(name: "Coffee", description: "Definition for Coffee type products.");

            coffeeDefinition.ProductDefinitionFields = new List<ProductDefinitionFieldEntity>()
            {
                CreateProductDefinitionField(shortTextDataType, "OriginCountry", false, false),
                CreateProductDefinitionField(shortTextDataType, "CoffeeTaste", false, false),
                CreateProductDefinitionField(shortTextDataType, "TestDoesNotExsist", false, false),              
            };

            dbContext.Add(coffeeDefinition);

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private ProductDefinitionEntity CreateCoffeeProductDefinition(string name, string description)
        {
            return new ProductDefinitionEntity
            {
                Name = name,
                Description = description,
                Deleted = false
            };
        }

        private ProductDefinitionFieldEntity CreateProductDefinitionField(DataTypeEntity dataType, string name, bool isMultilingual, bool isVariantProperty)
        {
            return new ProductDefinitionFieldEntity
            {
                Name = name,
                Deleted = false,
                Multilingual = isMultilingual,
                DisplayOnSite = true,
                RenderInEditor = true,
                IsVariantProperty = isVariantProperty,
                DataType = dataType
            };
        }
    }
}
