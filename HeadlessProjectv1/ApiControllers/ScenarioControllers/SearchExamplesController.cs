using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace HeadlessProjectv1.ApiControllers.ScenarioControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchExamplesController : ControllerBase
    {
        private readonly IIndex<ProductSearchModel> _indexProduct;
        private readonly IIndex<CatalogSearchModel> _indexCatalog;

        public SearchExamplesController(IIndex index, IIndex<CatalogSearchModel> indexCatalog, IIndex<ProductSearchModel> indexProduct)
        {
            _indexCatalog = indexCatalog;
            _indexProduct = indexProduct;
        }

        [HttpGet("GetProductNameForA001")]
        public string GetProductNameForA001(CancellationToken token)
        {
            var language = new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };

            var indexSearch = _indexProduct.AsSearchable(language.Culture);

            var productResultSet = indexSearch.Where(x => x.Sku == "A001")
                .ToResultSet(token).Result;

            var result = productResultSet.FirstOrDefault()?.Name;

            return result ??= "No product found";
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