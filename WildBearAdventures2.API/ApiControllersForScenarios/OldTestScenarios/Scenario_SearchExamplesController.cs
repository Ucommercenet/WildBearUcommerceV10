using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace WildBearAdventures.API.ApiControllersForScenarios.OldTestScenarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class Scenario_SearchExamplesController : ControllerBase
    {
        private readonly IIndex<ProductSearchModel> _indexProduct;
        private readonly IIndex<CatalogSearchModel> _indexCatalog;

        public Scenario_SearchExamplesController(IIndex index, IIndex<CatalogSearchModel> indexCatalog, IIndex<ProductSearchModel> indexProduct)
        {
            _indexCatalog = indexCatalog;
            _indexProduct = indexProduct;
        }
               

        [SwaggerOperation(Summary = "Gets the first Catalog, which on a fresh install would be the Ucommerce Catalog.")]
        [HttpGet("GetDefaultCatalog")]
        public CatalogSearchModel? GetDefaultCatalog(CancellationToken token)
        {
            var culture = new CultureInfo("da-DK");

            var searchable = _indexCatalog.AsSearchable(culture);

            var result = searchable.FirstOrDefault(token).Result;

            return result;
        }
    }
}