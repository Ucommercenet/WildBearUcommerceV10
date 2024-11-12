using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ucommerce.Web.BackOffice.Pipelines.Category.AddProductsToCategory;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;

namespace Ucommerce.API.ApiControllers.Sandbox
{
    /// <summary>
    /// The "_" in the class name ensures that controller is the top controller when using Swagger.    
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class __StartUpScenario : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly ProductUtilities _productUtilities;

        public __StartUpScenario(UcommerceDbContext ucommerceDbContext, ProductUtilities productUtilities)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productUtilities = productUtilities;
        }

        [HttpPost("RunMainStartUpSequence")]
        public async Task<IActionResult> _RunStartUpSequence(CancellationToken cancellationToken)
        {

            //Will be used as a theme for this Sequence
            var productName = "Coffee";
            var categoryName = "Drinks";
            var productDefinitionName = $"{productName} And other hot beverages";
            var culture = "da-DK";


            CreateNewCategory(categoryName);
            CreateNewProductDefinition(productDefinitionName);
            CreateNewProduct(definitionName: productDefinitionName, productName: productName, culture: culture);
            AddProductToCategory(categoryName, productName);


            AddShortTextFieldToProductDefinition(productDefinitionName);



            return Ok();
        }

        
        private void AddProductToCategory(string categoryName, string productName)
        {
            var categoryEntity = _ucommerceDbContext.Set<CategoryEntity>().Single(x => x.Name == categoryName);
            var productEntity = _ucommerceDbContext.Set<ProductEntity>().Single(x => x.Name == productName);

            categoryEntity.CategoryProductRelations.Add(new CategoryProductRelationEntity
            {
                Category = categoryEntity,
                Product = productEntity
            });
        }

        
        [HttpPost("CreateNewCategory")]
        public IActionResult CreateNewCategory(string name)
        {
            var categoryExists = _ucommerceDbContext.Set<CategoryEntity>().Any(x => x.Name == name);

            if (categoryExists is true)
            {
                return Conflict("CategoryEntity already exists");
            }
            _productUtilities.CreateCategory(name);
            return Ok();
        }

        //TODO: add HttpPost on all IActionResult methods
        public IActionResult CreateNewProductDefinition(string definitionName)
        {
            //Will Create definition if it does not exist          
            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == definitionName);
            if (productDefinitionExists is true)
            {
                return Conflict("ProductDefinitionEntity already exists");
            }
            _productUtilities.CreateProductDefinition(definitionName);
            return Ok("");
        }
        
        public IActionResult CreateNewProduct(string definitionName, string productName, string culture)
        {
            //Step 1: Find ProductDefinitionEntity
            var productDefinition = _ucommerceDbContext.Set<ProductDefinitionEntity>()
                .FirstOrDefault(x => x.Name == definitionName);
            if (productDefinition == null)
            {
                return NotFound("ProductDefinitionEntity not found");
            }

            //Step 2: Create ProductEntity
            var randomLetterAndNumber = GenerateRandomLetterAndNumber();
            //Improve Todo: add some check if the productName or sku already exists

            var productEntity = _productUtilities.CreateRegularProduct(
                name: productName + randomLetterAndNumber,
                sku: randomLetterAndNumber,
                productDefinition: productDefinition,
                culture: culture
                );
            _ucommerceDbContext.Add(productEntity);


            _productUtilities.CreateCategoryProductRelation(new List<ProductEntity> { productEntity }, _ucommerceDbContext.Set<CategoryEntity>().First());

            //Step 4: Add Product to Category
            //_ucommerceDbContext.Set<CategoryProductRelationEntity>().Add(categoryProductRelation);

            //Step 5: Save











            return Ok();

        }
                
        public IActionResult AddShortTextFieldToProductDefinition(string productDefinitionName)
        {
            
            var productDefinition = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == productDefinitionName).FirstOrDefault();  //productDefinitionName is not unique

            if (productDefinition == null)
            { return NotFound("definition not found"); }

            var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>()
              .FirstOrDefault(x => x.DefinitionName == "ShortText") ?? throw new Exception("ShortText DataType not found");


            var productDefinitionFieldEntity = new ProductDefinitionFieldEntity
            {
                Name = "Taste",
                Deleted = false,
                Multilingual = false,
                DisplayOnSite = true,
                RenderInEditor = true,
                IsVariantProperty = false,
                DataType = shortTextDataType
            };

            productDefinition.ProductDefinitionFields.Add(productDefinitionFieldEntity);            
            _ucommerceDbContext.SaveChanges();

            return Ok();

        }         

        private string GenerateRandomLetterAndNumber()
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
