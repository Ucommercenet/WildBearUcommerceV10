using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using WildBearAdventures.API.WildBearDemoProducts;
using MimeKit.Cryptography;
using Ucommerce.Web.Infrastructure.Persistence.Mapping;
using Ucommerce.Web.Infrastructure.Persistence.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WildBearAdventures.API.ApiControllersForScenarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class _WildBearScenariosController : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly DemoEntitiesGenerator _demoEntitiesGenerator;

        public _WildBearScenariosController(UcommerceDbContext ucommerceDbContext, DemoEntitiesGenerator demoEntitiesGenerator)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _demoEntitiesGenerator = demoEntitiesGenerator;
        }


        //Cound this be a PUT request!?
        //PUT requests are idempotent, meaning that sending the same request multiple times will have the same effect as sending it once.               
        [HttpPost("Startup")]
        public async Task<IActionResult> Post(CancellationToken cancellationToken)
        {
            var endpointMessage = string.Empty;

            const string WildCoffeeDefinitionName = "WildCoffee";
            const string ProductDefinitionDescription = "Definition for Coffee type products";
            //const string ProductDefinitionFieldName = "OriginCountry";


            var productDefinitionExists = _ucommerceDbContext.Set<ProductDefinitionEntity>().Any(x => x.Name == WildCoffeeDefinitionName);
            if (productDefinitionExists is false)
            {
                //***Adds CoffeeProductDefinition
                var WildCoffeeProductDefinition = new ProductDefinitionEntity()
                {
                    Name = WildCoffeeDefinitionName,
                    Description = ProductDefinitionDescription,
                    Deleted = false
                };

                _ucommerceDbContext.Set<ProductDefinitionEntity>().Add(WildCoffeeProductDefinition);
                endpointMessage += $"Definition: {WildCoffeeProductDefinition.Name} was added";
                _ucommerceDbContext.SaveChanges();
            }



            //***Adds DemoCoffeeProducts
            var WildCoffeeProducts = await _demoEntitiesGenerator.CreateCoffeeProducts(cancellationToken);
            _ucommerceDbContext.Set<ProductEntity>().AddRange(WildCoffeeProducts);



            //Just for debug
            foreach (var product in WildCoffeeProducts)
            { endpointMessage += $"WildCoffeeProducts {product.Name} was added"; };

            //Always last saveChanges
            _ucommerceDbContext.SaveChanges();
            return Ok(endpointMessage);
        }


        //[HttpPost("Sandbox")]
        //public async Task<IActionResult> Post(CancellationToken cancellationToken)
        //{
        //}




    }
}
