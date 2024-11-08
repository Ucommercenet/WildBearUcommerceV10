using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Ucommerce.Web.BackOffice.Constants;
using Ucommerce.Web.Core.Constants;
using Elastic.Clients.Elasticsearch;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ucommerce.API.ApiControllers.Sandbox
{


    /// <summary>
    /// The "_" in the class name ensures that controller is the top controller when using Swagger.    
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class _SandboxScenariosController : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly ProductUtilities _productUtilities;

        public _SandboxScenariosController(UcommerceDbContext ucommerceDbContext, ProductUtilities demoEntitiesGenerator)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productUtilities = demoEntitiesGenerator;
        }        

        [HttpPost("NewProductDefinition")]
        public IActionResult NewProductDefinition(string definitionName, CancellationToken cancellationToken)
        {

            //step 1 Creating the definition
            var definition = new ProductDefinitionEntity()
            {
                Name = definitionName,
                Description = "Definition for any type of products",
                Deleted = false,
            };

            _ucommerceDbContext.Add(definition);
            _ucommerceDbContext.SaveChanges();


            return Ok();
        }

        [HttpPost("NewProductDefinitionField")]
        public IActionResult NewProductDefinitionField(string definitionName, string definitionFieldName, CancellationToken cancellationToken)
        {

            //step 1 find the definition from before
            var productDefinitionEntity = _ucommerceDbContext
               .Set<ProductDefinitionEntity>()
               .Include(x => x.ProductDefinitionFields)
               .Where(x => x.Name == definitionName).FirstOrDefault();

            //step 2 create a new definitionFieldEntity

            var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>()
             .FirstOrDefault(x => x.DefinitionName == "ShortText") ?? throw new Exception("ShortText DataType not found");

            var definitionFieldEntity = new ProductDefinitionFieldEntity
            {
                Name = definitionFieldName,
                Deleted = false,
                Multilingual = false,
                DisplayOnSite = true,
                RenderInEditor = true,
                IsVariantProperty = false,
                DataType = shortTextDataType
            };


            if (productDefinitionEntity.ProductDefinitionFields != null)
            {
                //Step 3 add the definitionFieldEntity to the new definition
                productDefinitionEntity.ProductDefinitionFields.Add(definitionFieldEntity);

                //Step 4 saves           
                _ucommerceDbContext.SaveChanges();
            }

            return Ok();
        }           


        [HttpPost("WIP_AddComplexTestProductDefinition")]
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
        /// CodeSnippets Endpoints for testing snippets of code.
        /// </summary>        
        #region CodeSnippets Endpoints

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
