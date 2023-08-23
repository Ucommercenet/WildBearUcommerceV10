using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IIndex _index;

        public ProductController(IIndex index)
        {
            _index = index;
        }

        //test with product A001
        [HttpGet("GetProductName")]
        public string GetProductName(string sku)
        {
            //Todo: Get Language
            var language = new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };

            var indexSearch = _index.AsSearchable<ProductSearchModel>(language.Culture);

            var productResultset = indexSearch.Where(x => x.Sku == sku)
                .ToResultSet(new CancellationToken()).Result;

            var output = productResultset.FirstOrDefault().Name;

            return output;
        }
    }
}