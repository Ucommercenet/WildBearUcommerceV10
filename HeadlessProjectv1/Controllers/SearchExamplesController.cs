using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchExamplesController : ControllerBase
    {
        private readonly IIndex _index;

        public SearchExamplesController(IIndex index)
        {
            _index = index;
        }


        [HttpGet("GetProductNameForA001")]
        public string GetProductNameForA001()
        {
            var language = new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };

            var indexSearch = _index.AsSearchable<ProductSearchModel>(language.Culture);

            var productResultSet = indexSearch.Where(x => x.Sku == "A001")
                .ToResultSet(new CancellationToken()).Result;


            var result = productResultSet.FirstOrDefault()?.Name;

            return result ??= "No product found";
        }


        [SwaggerOperation(Summary = "DOES NOT WORK! Because search is not feature complete. Try again in next alpha build")]
        [HttpGet("GetDefaultCatalog")]
        public CatalogSearchModel? GetDefaultCatalog()
        {
            var culture = new CultureInfo("da-DK");

            var searchable = _index.AsSearchable<CatalogSearchModel>(culture);

            var catalogSearchModel = searchable.FirstOrDefault(new CancellationToken()).Result;


            return catalogSearchModel;
        }


    }
}