using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ucommerce.Web.BackOffice.Pipelines.Category.AddProductsToCategory;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;

namespace Ucommerce.API.ApiControllers.Sandbox.InitializeExamples
{
    /// <summary>
    /// The "_" in the class name ensures that controller is the top controller when using Swagger.    
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Prefix with '_' will insure its placed first in swagger")]
    public class _InitializeProducts : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly ProductUtilities _productUtilities;


        public _InitializeProducts(UcommerceDbContext ucommerceDbContext, ProductUtilities productUtilities)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productUtilities = productUtilities;
        }

        [HttpPost("InitializeProductSetupSequence")]
        public IActionResult InitializeProductSetupSequence(CancellationToken cancellationToken)
        {

            //Names for the theme of this Sequence
            var productNameSeed = "SpecialCoffee6";
            var categoryName = "SpecialDrinks6";
            var productDefinitionName = $"{productNameSeed} And other hot beverages";
            var ProductDefinitionFieldName = "Taste";
            var culture = "da-DK";


            CreateNewCategory(categoryName);
            CreateNewProductDefinition(productDefinitionName);
            _ucommerceDbContext.SaveChanges();
            var fullProductName = CreateNewProduct(definitionName: productDefinitionName, productName: productNameSeed, culture: culture);
            _ucommerceDbContext.SaveChanges();
            AddProductToCategory(categoryName, fullProductName);
            AddShortTextFieldToProductDefinition(productDefinitionName, ProductDefinitionFieldName);

            _ucommerceDbContext.SaveChanges();

            return Ok($"Created a product named {productNameSeed} with the definition {productDefinitionName} and added it to {categoryName} category");
        }


        private void AddProductToCategory(string categoryName, string productName)
        {
            var categoryEntity = _ucommerceDbContext.Set<CategoryEntity>().Single(x => x.Name == categoryName);
            var productEntity = _ucommerceDbContext.Set<ProductEntity>().Single(x => x.Name == productName);


            var categoryProductRelation = new CategoryProductRelationEntity
            {
                Category = categoryEntity,
                Product = productEntity
            };

            _ucommerceDbContext.Set<CategoryProductRelationEntity>().Add(categoryProductRelation);

            // This will not work
            //categoryEntity.CategoryProductRelations.Add(categoryProductRelation);


        }

        private void CreateNewCategory(string name)
        {
            var categoryExists = _ucommerceDbContext.Set<CategoryEntity>().Any(x => x.Name == name);

            if (categoryExists)
            {
                throw new Exception("Category already exists");
            }

            _productUtilities.CreateCategory(name);
            return;
        }

        private void CreateNewProductDefinition(string definitionName)
        {
            //Will Create definition if it does not exist          
            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == definitionName);
            if (productDefinitionExists is true)
            {
                throw new Exception("ProductDefinitionEntity already exists");
            }
            _productUtilities.CreateProductDefinition(definitionName);
            Ok();
        }

        private string CreateNewProduct(string definitionName, string productName, string culture)
        {
            //Step 1: Find ProductDefinitionEntity
            var productDefinition = _ucommerceDbContext.Set<ProductDefinitionEntity>()
                .FirstOrDefault(x => x.Name == definitionName);
            if (productDefinition == null)
            {
                throw new Exception("ProductDefinitionEntity not found");
            }

            //Step 2: Create ProductEntity
            var randomLetterAndNumber = GenerateRandomLetterAndNumber();
            //Improve Todo: add some check if the productNameSeed or sku already exists

            var productEntity = _productUtilities.CreateRegularProduct(
                name: productName + randomLetterAndNumber,
                sku: randomLetterAndNumber,
                productDefinition: productDefinition,
                culture: culture);

            _ucommerceDbContext.Add(productEntity);

            return productEntity.Name;

        }

        private void AddShortTextFieldToProductDefinition(string productDefinitionName, string productDefinitionFieldName)
        {

            var productDefinition = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == productDefinitionName)
                .FirstOrDefault() ?? throw new Exception("definition not found");  //productDefinitionName is not unique

            var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>()
              .FirstOrDefault(x => x.DefinitionName == "ShortText") ?? throw new Exception("ShortText DataType not found");


            var productDefinitionFieldEntity = new ProductDefinitionFieldEntity
            {
                Name = productDefinitionFieldName,
                Deleted = false,
                Multilingual = false,
                DisplayOnSite = true,
                RenderInEditor = true,
                IsVariantProperty = false,
                DataType = shortTextDataType
            };

            productDefinition.ProductDefinitionFields.Add(productDefinitionFieldEntity);

            return;

        }

        private static string GenerateRandomLetterAndNumber()
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
