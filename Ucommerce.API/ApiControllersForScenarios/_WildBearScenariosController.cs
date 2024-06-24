using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using WildBearAdventures.API.WildBearDemoProducts;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WildBearAdventures.API.ApiControllersForScenarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class _WildBearScenariosController : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly DemoToolbox _demoToolbox;

        public _WildBearScenariosController(UcommerceDbContext ucommerceDbContext, DemoToolbox demoEntitiesGenerator)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _demoToolbox = demoEntitiesGenerator;
        }

         [HttpPost("_RunStartUpSequence")]
        public async Task<IActionResult> RunStartUpSequence(CancellationToken cancellationToken)
        {
            await StartupCategories(cancellationToken);

            return Ok();
        }

     


        [HttpPost("StartupCategories")]
        public async Task<IActionResult> StartupCategories(CancellationToken cancellationToken)
        {
            const string CategoryName = "Drinks";
            _demoToolbox.CreateCategory(CategoryName);

            return Ok();
        }

        [HttpPost("StartupProducts")]
        public async Task<IActionResult> StartupProducts(CancellationToken cancellationToken)
        {
            var endpointMessage = string.Empty;
            var wildCoffeeDefinitionName = "WildCoffee";

            //Will Create ProductDefinition if it does not exist          
            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == wildCoffeeDefinitionName);
            if (productDefinitionExists is false)
            {
                _demoToolbox.CreateProductDefinition(wildCoffeeDefinitionName);
                endpointMessage += $"Definition: {wildCoffeeDefinitionName} was added";
            }

            //***Adds DemoCoffeeProducts
            var WildCoffeeProducts = await _demoToolbox.CreateCoffeeProducts(cancellationToken);


            //Just for debug
            foreach (var product in WildCoffeeProducts)
            { endpointMessage += $"WildCoffeeProducts {product.Name} was added"; };

            return Ok(endpointMessage);

        }

        /// <summary>
        /// The added field wil be of type ShortText
        /// </summary>        
        [HttpPost("AddProductDefinition")]
        public IActionResult AddProductDefinition(string nameOfField = "CoffeeAroma")
        {
            var wildCoffeeDefinitionName = "WildCoffee";

            var wildCoffeeProductDefinitionEntity = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == wildCoffeeDefinitionName).FirstOrDefault();
               

            if (wildCoffeeProductDefinitionEntity == null)
            { return NotFound("wildCoffeeDefinition not found"); }                       

              var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>()
                .FirstOrDefault(x => x.DefinitionName == "ShortText") ?? throw new Exception("ShortText DataType not found");


            var definitionField =  CreateProductDefinitionField(shortTextDataType, nameOfField, false, false);   
            
            wildCoffeeProductDefinitionEntity.ProductDefinitionFields.Add(definitionField);
            
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
