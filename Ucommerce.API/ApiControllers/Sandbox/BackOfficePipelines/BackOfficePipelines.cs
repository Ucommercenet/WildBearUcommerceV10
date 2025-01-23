using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.BackOffice.Pipelines.Category.CreateCategory;
using Ucommerce.Web.Common.Extensions;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Pipelines;
using Microsoft.AspNetCore.Authorization;
using Ucommerce.Web.BackOffice.Authentication;
using Ucommerce.Web.BackOffice.Middlewares;
using static Ucommerce.Web.Core.Constants.FieldIdConstants;

namespace Ucommerce.API.ApiControllers.Sandbox.BackOfficePipelines
{
    [Route("api/[Controller]")]
    [ApiController]
    //[Authorize(Policy = AuthenticationConstants.BACKOFFICE_POLICY_NAME)] not needed
    //[MiddlewareFilter(typeof(UcommerceAuthorizationContextPipeline))]
    public class BackOfficePipelinesController : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly IPipeline<CreateCategoryInput, CreateCategoryOutput> _createCategoryPipeline;

        public BackOfficePipelinesController(UcommerceDbContext ucommerceDbContext, IPipeline<CreateCategoryInput, CreateCategoryOutput> createCategoryPipeline)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _createCategoryPipeline = createCategoryPipeline;
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

        
        [HttpPost("ExecuteOrderPipeline")]
        public async Task<IActionResult> ExecuteOrderPipeline(CancellationToken cancellationToken, string categoryName = "Hot Drinks")
        {
            return Ok();
        }


    }
}
