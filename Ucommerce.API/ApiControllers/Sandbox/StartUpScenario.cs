using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            CreateNewCategory();
            CreateNewProductDefinition();
            
            //await CreateProductsWithRandomName(cancellationToken);
            //AddProductDefinitionField();

            return Ok();
        }

        [HttpPost("CreateNewCategory")]
        public IActionResult CreateNewCategory(string name = "Drinks")
        {
            var productDefinitionExists = _ucommerceDbContext.Set<CategoryEntity>().Any(x => x.Name == name);

            if (productDefinitionExists is true)
            {
                return Conflict("CategoryEntity already exists");
            }
            _productUtilities.CreateCategory(name);
            return Ok();
        }

        [HttpPost("CreateNewProductDefinition")]
        public IActionResult CreateNewProductDefinition(string definitionName = "Coffee")
        {
            //Will Create definition if it does not exist          
            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == definitionName);
            if (productDefinitionExists is true)
            {
                return Conflict("ProductDefinitionEntity already exists");
            }
            _productUtilities.CreateProductDefinition(definitionName);
            return Ok();
        }



        [HttpPost("CreateProductsWithRandomName")]
        public async Task<IActionResult> CreateProductsWithRandomName(CancellationToken cancellationToken)
        {         
            var WildCoffeeProducts = await _productUtilities.CreateCoffeeProducts(cancellationToken);

                       
            return Ok();

        }


        /// <summary>
        /// The added field wil be of type ShortText
        /// </summary>        
        [HttpPost("AddProductDefinitionField")]
        public IActionResult AddProductDefinitionField(string nameOfField = "CoffeeAroma")
        {
            var wildCoffeeDefinitionName = "Tea";

            var wildCoffeeProductDefinitionEntity = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == wildCoffeeDefinitionName).FirstOrDefault();


            if (wildCoffeeProductDefinitionEntity == null)
            { return NotFound("wildCoffeeDefinition not found"); }



            var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>()
              .FirstOrDefault(x => x.DefinitionName == "ShortText") ?? throw new Exception("ShortText DataType not found");


            var shortTextDefinitionField = CreateProductDefinitionField(shortTextDataType, nameOfField, false, false);

            wildCoffeeProductDefinitionEntity.ProductDefinitionFields.Add(shortTextDefinitionField);

            //_ucommerceDbContext.Add(definitionField);
            _ucommerceDbContext.SaveChanges();

            return Ok();

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
