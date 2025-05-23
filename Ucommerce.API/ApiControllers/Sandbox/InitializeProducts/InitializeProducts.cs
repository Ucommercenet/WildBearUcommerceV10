using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using Ucommerce.API.Products;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Web.BackOffice.Pipelines.Category.AddProductsToCategory;
using Ucommerce.Web.Common.Extensions;
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
        private readonly IIndexer<ProductEntity> _productIndexer;
        private readonly IIndexer<CategoryEntity> _categoryIndexer;


        public _InitializeProducts(UcommerceDbContext ucommerceDbContext, ProductUtilities productUtilities, IIndexer<ProductEntity> indexer, IIndexer<CategoryEntity> categoryIndexer)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productUtilities = productUtilities;
            _productIndexer = indexer;
            _categoryIndexer = categoryIndexer;
        }

        //Remark all default values are just for easier testing in swagger, change to fit your needs
        [HttpPost("SetUserDefinedFieldValueOnProduct")]
        public IActionResult SetUserDefinedFieldValueOnProduct(
            string productName = "CoffeeTest6I1", string productPropertyEntityName = "Taste and flavor", string newValue = "Sweet but also bitter")
        {

            var product = _ucommerceDbContext.Products.First(x => x.Name == productName);

            //Remark remember to use Include or ProductDefinitionFields will be null
            var definition = _ucommerceDbContext.Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Single(x => x.Guid == product.DefinitionGuid);

            //Remark productPropertyEntityName is not unique, this might change in the future, because of feedback.
            var definitionField = definition.ProductDefinitionFields
                .Where(x => x.Name == productPropertyEntityName)
                .First();

            var productPropertyEntity = _ucommerceDbContext.Set<ProductPropertyEntity>()
                .Where(x => x.ProductId == product.Id)
                .Where(x => x.ProductDefinitionFieldId == definitionField.Id)
                .First();

            productPropertyEntity.Value = newValue;
            _ucommerceDbContext.SaveChanges();

            return Ok("UserDefinedFieldValueOnProduct updated");

        }

        [HttpPost("InitializeProductSetupSequence")]
        public async Task<IActionResult> InitializeProductSetupSequence(CancellationToken cancellationToken)
        {

            //Names for the theme of this Sequence
            var productNameSeed = "Caramel Coffee";
            var categoryName = "NewDrinks";
            var productDefinitionName = $"{productNameSeed} And other hot beverages";
            var ProductDefinitionFieldName = "Taste and flavor";
            var culture = "da-DK";

            //Step 1: Create a ProductDefinition and add a field to it
            var productDefinitionEntity = CreateNewProductDefinition(productDefinitionName);
            AddShortTextFieldToProductDefinition(productDefinitionEntity, ProductDefinitionFieldName);
            _ucommerceDbContext.SaveChanges();
            /* SaveChanges is necessary here because, without it:
            EF Core will try and save the product (See next SaveChanges) before it know about the ProductDefinition which will result in a failure.
            Why EF Core is not is not creating the entities in SQL, in the same order as we have initialized them is, is a good question!
            */

            //Step 2: Create a Product and put it in a Category
            var product = CreateNewProduct(productDefinitionGuid: productDefinitionEntity.Guid, productName: productNameSeed, culture: culture);
            var category = CreateCategory(categoryName);
            AddProductToCategory(category, product);


            //Step 3: add the product and category to the index
            await _productIndexer.Index(product, cancellationToken);
            await _categoryIndexer.Index(category, cancellationToken);

            _ucommerceDbContext.SaveChanges(); //Se comment above


            return Ok($"Created a product named {productNameSeed} with the definition {productDefinitionName} and added it to {categoryName} category");
        }


        private void AddProductToCategory(CategoryEntity category, ProductEntity product)
        {
            var categoryProductRelation = new CategoryProductRelationEntity
            {
                Category = category,
                Product = product
            };

            _ucommerceDbContext.Set<CategoryProductRelationEntity>().Add(categoryProductRelation);
        }       

        private CategoryEntity CreateCategory(string name)
        {
            var categoryExists = _ucommerceDbContext.Set<CategoryEntity>().Any(x => x.Name == name);

            if (categoryExists)
            {
                var category = _ucommerceDbContext.Set<CategoryEntity>().Single(x => x.Name == name);

                return category;
            }

            var newCategory = _productUtilities.CreateCategory(name); 

            _ucommerceDbContext.Add(newCategory);

            return newCategory;
        }

        private ProductDefinitionEntity CreateNewProductDefinition(string definitionName)
        {
            //Will Create definition if it does not exist          
            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == definitionName);
            if (productDefinitionExists is true)
            {
                throw new Exception("ProductDefinitionEntity already exists");
            }

            var productDefinitionEntity = _productUtilities.CreateProductDefinition(definitionName);
            _ucommerceDbContext.Add(productDefinitionEntity);

            return productDefinitionEntity;


        }

        private ProductEntity CreateNewProduct(Guid productDefinitionGuid, string productName, string culture)
        {



            var randomLetterAndNumber = GenerateRandomLetterAndNumber();
            //Improve Todo: add some check if the productNameSeed or sku already exists

            var productEntity = _productUtilities.CreateRegularProduct(
                name: productName + randomLetterAndNumber,
                sku: randomLetterAndNumber,
                productDefinitionGuid: productDefinitionGuid,
                culture: culture);

            _ucommerceDbContext.Add(productEntity);

            return productEntity;

        }

        private void AddShortTextFieldToProductDefinition(ProductDefinitionEntity productDefinition, string productDefinitionFieldName)
        {

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
