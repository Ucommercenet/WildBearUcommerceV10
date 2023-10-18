using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Common.Extensions;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IIndex<ProductSearchModel> _indexProduct;
        private readonly IIndex<CategorySearchModel> _indexCategory;

        public ProductController(IIndex<CategorySearchModel> indexCategory, IIndex<ProductSearchModel> indexProduct)
        {
            _indexCategory = indexCategory;
            _indexProduct = indexProduct;

        }


        [SwaggerOperation(Summary = "Example: categoryName = 'WildCoffee', Culture = 'da-DK'")]
        [HttpGet("{categoryName}")]
        public async Task<IActionResult> GetAllProductsFromCategoryName(string categoryName, string? cultureInput, CancellationToken token)
        {

            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }


            var category = await _indexCategory.AsSearchable(culture).Where(x => x.Name == categoryName).FirstOrDefault(token);
            if (category == null) { return NotFound(); }

            var resultSet = await _indexProduct.AsSearchable(culture).Where(x => x.CategoryIds.Contains(category.Id)).ToResultSet(token);
            if (resultSet.Any() == false) { return NotFound(); }


            return Ok(resultSet.Results);
        }







    }
}