using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;
using System.Globalization;
using Ucommerce.Web.BackOffice.Middlewares;
using Ucommerce.Web.BackOffice.Pipelines.Category.CreateCategory;
using Ucommerce.Web.BackOffice.Pipelines.Order.UpdateOrder;
using Ucommerce.Web.Common.Extensions;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace Ucommerce.API.ApiControllers.Sandbox.BackOfficePipelines
{
    [Route("api/[Controller]")]
    [ApiController]
    //[Authorize(Policy = AuthenticationConstants.BACKOFFICE_POLICY_NAME)] not needed
    [MiddlewareFilter(typeof(UcommerceAuthorizationContextPipeline))]
    public class BackOfficePipelinesController : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly IPipeline<CreateCategoryInput, CreateCategoryOutput> _createCategoryPipeline;
        private readonly IPipeline<UpdateOrderInput, UpdateOrderOutput> _updateOrderPipeline;   

        public BackOfficePipelinesController(UcommerceDbContext ucommerceDbContext, IPipeline<CreateCategoryInput, CreateCategoryOutput> createCategoryPipeline, IPipeline<UpdateOrderInput, UpdateOrderOutput> updateOrderPipeline)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _createCategoryPipeline = createCategoryPipeline;
            _updateOrderPipeline = updateOrderPipeline;
        }

        [HttpPost("ExecuteCreateCategoryPipeline")]
        public async Task<IActionResult> ExecuteCreateCategoryPipeline(CancellationToken cancellationToken, string categoryName = "Hot Drinks")
        {
            if (categoryName.IsNullOrWhiteSpace())
            {
                categoryName = "New test Category";
            }

            var culture = "da-DK";

            var defaultCategoryDefinition = _ucommerceDbContext.Set<DefinitionEntity>()
              .Where(x => x.Name == "Default Category Definition").First();

            var catalogGuid = _ucommerceDbContext.Set<CatalogEntity>().FirstOrDefault(x => x.Name == "Ucommerce")?.Guid;
            if (catalogGuid == null)
            {
                //If none is name Ucommerce just find the first catalog
                catalogGuid = _ucommerceDbContext.Set<CatalogEntity>().First().Guid;
            }


            var pipeLineInput = new CreateCategoryInput(
                DefinitionGuid: defaultCategoryDefinition.Guid,
                Name: categoryName,
                CatalogId: catalogGuid.Value,
                SortOrder: 0,
                CultureInfo: new CultureInfo(culture));

            var result = await _createCategoryPipeline.Execute(pipeLineInput, cancellationToken);

            //result.EnsureSuccess(); //For debugging its nice to not use this.

            return Ok();
        }


        [HttpPost("Execute_UpdateOrderPipeline")]
        public async Task<IActionResult> ExecuteUpdateOrderPipeline(CancellationToken cancellationToken)
        {
            var culture = "da-DK";

            var order = _ucommerceDbContext.Set<OrderEntity>()
              .Where(x => x.OrderNumber == "WEB-5").First();

            var cancelledOrderStatus = _ucommerceDbContext.Set<OrderStatusEntity>()
              .Where(x => x.Name == "Cancelled").First();

            var jObject = new JObject()
            {
                { "guid", cancelledOrderStatus.Guid }, //OrderStatusGuid
                { "value", order.Guid } //OderGuid    
            };

            var updateProperties = new Dictionary<string, JToken>()
            {
                { "status", jObject }
            }
            .ToImmutableDictionary();

            var updateOrderInput = new UpdateOrderInput(
                OrderGuid: order.Guid,
                Culture: new CultureInfo(culture),
                UpdateProperties: updateProperties
            );

            var response = await _updateOrderPipeline.Execute(updateOrderInput, cancellationToken);


            return Ok();
        }


    }
}
