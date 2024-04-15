using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using WildBearAdventures.API.WildBearDemoProducts;
using MimeKit.Cryptography;
using Ucommerce.Web.Infrastructure.Persistence.Mapping;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
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
        [HttpPost("UpdateProductDefinition")]
        public IActionResult UpdateProductDefinition(string nameOfField = "CoffeeType")
        {
            var wildCoffeeDefinitionName = "WildCoffee";

            var wildCoffeeProductDefinitionEntity = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == wildCoffeeDefinitionName).FirstOrDefault();
               

            if (wildCoffeeProductDefinitionEntity == null)
            { return NotFound("wildCoffeeDefinition not found"); }                       

            _demoToolbox.AddShortTextFieldToProductDefinition(wildCoffeeProductDefinitionEntity.ProductDefinitionFields,nameOfField);

            return Ok();

        }
        

    }
}
