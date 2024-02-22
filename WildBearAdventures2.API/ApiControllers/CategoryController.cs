using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Common.Extensions;
using Ucommerce.Web.WebSite.Controllers;

namespace WildBearAdventures.API.ApiControllers
{
    [Route("api/[Controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly IIndex<CategorySearchModel> _indexCategory;
        private readonly IIndex<CatalogSearchModel> _indexCatalog;


        public CategoryController(IIndex<CategorySearchModel> indexCategory, IIndex<CatalogSearchModel> indexCatalog)
        {
            _indexCategory = indexCategory;
            _indexCatalog = indexCatalog;
        }


        [HttpGet("GetCategoryByName")]
        public async Task<IActionResult> GetCategoryByName(string name, string? cultureInput, CancellationToken token)
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


            return result is null ? NotFound() : Ok(result);

        }

        [HttpGet("GetCategoryByGuid")]
        public async Task<IActionResult> GetCategoryByGuid(Guid searchGuid, string? cultureInput, CancellationToken token)
        {
            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }


            var searchCategory = await _indexCategory.AsSearchable(culture).Where(category => category.Id == searchGuid)
               .ToResultSet(token);
            var result = searchCategory.SingleOrDefault();


            return result is null ? NotFound() : Ok(result);
        }

        //Optimize: is this fast? or should I solved this in WildbearClient
        [HttpGet("GetOnlyGuidByName")]
        public async Task<IActionResult> GetOnlyGuidByName(string searchName, string? cultureInput, CancellationToken token)
        {
            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }


            var searchCategory = await _indexCategory.AsSearchable(culture).Where(category => category.Name == searchName)
               .ToResultSet(token);
            var result = searchCategory.SingleOrDefault()?.Id;


            return result is null ? NotFound() : Ok(result);
        }




        [HttpGet("GetAllCategoriesFromCatalog")]
        public async Task<IActionResult> GetAllCategoriesFromCatalog(string? catalogName, string? cultureInput, CancellationToken token)
        {
            //Culture
            if (cultureInput.IsNullOrWhiteSpace())
            { cultureInput = "da-DK"; }
            var culture = new CultureInfo(cultureInput);
            if (culture == null)
            { return NotFound(); }

            //Catalog: A common use case is that there is only 1 store with 1 catalog. catalog is therefor optional and in this case the first will be 

            //Optimize: by not searching only if catalogName is not null or whitespace
            var targetCatalog = await _indexCatalog.AsSearchable(culture).Where(x => x.Name == catalogName).FirstOrDefault(token)
                ?? await _indexCatalog.AsSearchable(culture).FirstOrDefault(token);


            var searchCategories = await _indexCategory
                .AsSearchable(culture)
                .Where(category => category.CatalogId == targetCatalog.Id)
                .ToResultSet(token);

            return Ok(searchCategories);





        }


    }
}
