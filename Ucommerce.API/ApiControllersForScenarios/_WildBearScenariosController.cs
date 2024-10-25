using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Ucommerce.API.WildBearDemoProducts;
using System.Diagnostics;
using Ucommerce.Web.BackOffice.Constants;
using Ucommerce.Web.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ucommerce.API.ApiControllersForScenarios
{


    /// <summary>
    /// The "_" in the class name ensures that controller is the top controller when using Swagger.
    /// Same for methods
    /// </summary>

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
        public async Task<IActionResult> _RunStartUpSequence(CancellationToken cancellationToken)
        {
            StartupCategories(cancellationToken);
            await StartupProducts(cancellationToken);
            AddProductDefinition();

            return Ok();
        }




        [HttpPost("StartupCategories")]
        public IActionResult StartupCategories(CancellationToken cancellationToken)
        {
            _demoToolbox.CreateCategory("Drinks");

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


            var shortTextDefinitionField = CreateProductDefinitionField(shortTextDataType, nameOfField, false, false);

            wildCoffeeProductDefinitionEntity.ProductDefinitionFields.Add(shortTextDefinitionField);

            //_ucommerceDbContext.Add(definitionField);
            _ucommerceDbContext.SaveChanges();

            return Ok();

        }

        [HttpPost("AddComplexTestProductDefinition")]
        public IActionResult AddComplexProductDefinitionFields(string nameOfField = "CoffeeComplexTestImagePickerMultiSelect")
        {

            var wildCoffeeDefinitionName = "WildCoffee";

            var wildCoffeeProductDefinitionEntity = _ucommerceDbContext
                .Set<ProductDefinitionEntity>()
                .Include(x => x.ProductDefinitionFields)
                .Where(x => x.Name == wildCoffeeDefinitionName).FirstOrDefault();


            if (wildCoffeeProductDefinitionEntity == null)
            { return NotFound("wildCoffeeDefinition not found"); }



            var CoffeeComplexTypeTest = _ucommerceDbContext.Set<DataTypeEntity>()
              .FirstOrDefault(x => x.DefinitionName == "ImagePickerMultiSelect") ?? throw new Exception("DataType not found");

            


            var definitionFieldEntity = CreateProductDefinitionField(CoffeeComplexTypeTest, nameOfField, false, false);

            wildCoffeeProductDefinitionEntity.ProductDefinitionFields.Add(definitionFieldEntity);


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


        /// <summary>
        /// Sandbox Endpoints for testing snippets of code.
        /// </summary>        
        #region Sandbox Endpoints

        //Create new endpoint that gets alle DataTypeEntity

        [HttpGet("GetAllDataTypes")]
        public IActionResult GetAllDataTypes()
        {


            var dataTypes = _ucommerceDbContext.Set<DataTypeEntity>().ToList();
            return Ok(dataTypes);


        }

        //Create new endpoint that gets all PriceGroups
        [HttpGet("GetAllPriceGroups")]
        public IActionResult GetAllPriceGroups()
        {
            var priceGroups = _ucommerceDbContext.Set<PriceGroupEntity>().ToList();
            return Ok(priceGroups.FirstOrDefault());
        }



        [HttpGet("GetChangeOrderDate")]
        public IActionResult ChangeOrderDate_AddDaysToCompletedDate(string orderNumberId)
        {
            var endpointMessage = string.Empty;

            var order = _ucommerceDbContext.Set<OrderEntity>().FirstOrDefault(x => x.OrderNumber == orderNumberId);
            if (order == null)
            {
                return NotFound("Order not found");
            }



            order.CompletedDate = order.CompletedDate.Value.AddDays(5);



            _ucommerceDbContext.SaveChanges();

            return Ok(order.CompletedDate);
        }
        #endregion

    }
}
