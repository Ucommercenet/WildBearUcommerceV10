using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Common.Extensions;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[Controller]")]
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

        [HttpGet("GetTest")]
        public async Task<IActionResult> GetTest(string name, string? cultureInput, CancellationToken token)
        {
            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }


            var searchCategory = await _indexCategory.AsSearchable(culture).Where(category => category.Name == name)
               .ToResultSet(token);
            var result = searchCategory.SingleOrDefault();


            return (result is null) ? NotFound() : Ok(result);

        }


        //TEST with Wild Coffee: 7040940e-eab1-4a72-85b5-867905b7d94a        
        [SwaggerOperation(Summary = "Example: 7040940e-eab1-4a72-85b5-867905b7d94a")]
        [HttpGet("GetAllProductsFromCategoryGuid")]
        public async Task<IActionResult> GetAllProductsFromCategoryGuid(Guid categoryId, string? cultureInput, CancellationToken token)
        {
            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }

            var category = await _indexCategory.AsSearchable(culture).Where(x => x.Id == categoryId).FirstOrDefault(token);
            if (category == null) { return NotFound(); }

            var resultSet = await _indexProduct.AsSearchable(culture).Where(x => x.CategoryIds.Contains(category.Id)).ToResultSet(token);
            if (resultSet.Any() == false) { return NotFound(); }


            return Ok(resultSet.Results);
        }


    }
}