using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Immutable;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace WildBearAdventures.API.ApiControllersForScenarios.OldTestScenarios
{

    [Route("api/[controller]")]
    [ApiController]
    public class Scenario_CoffeeController : ControllerBase
    {
        //Easy Coffee scenario: This is anti pattern! But lets make it work then move the controllers out of this class

        private readonly IIndex<ProductSearchModel> _indexProduct;
        private readonly IIndex<CategorySearchModel> _indexCategory;

        private readonly Language _language;

        public Scenario_CoffeeController(IIndex<ProductSearchModel> indexProduct, IIndex<CategorySearchModel> indexCategory)
        {
            _indexProduct = indexProduct;
            _indexCategory = indexCategory;

            _language = new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };
        }

        [SwaggerOperation(Summary = "Returns all Products from Category 'WildCoffee' including variants")]
        [HttpGet("GetAllProductsFromCategoryWildCoffee")]
        public async Task<ResultSet<ProductSearchModel>> GetAllProductsFromCategoryWildCoffee(CancellationToken token)
        {
            var category = await _indexCategory.AsSearchable(_language.Culture).Where(x => x.Name == "WildCoffee").FirstOrDefault(token);

            // ***THIS IS NOT ProductIds but Product RelationIds***
            //var productRelationGuids = category?.ProductIds;


            //Testing the sort by Displayname
            var query = _indexProduct.AsSearchable(_language.Culture).Where(x => x.CategoryIds.Contains(category.Id));

            var orderedResult = query.OrderBy(x => x.DisplayName);


            var finalResuelt = await orderedResult.OrderByDescending(x => x.ShortDescription).ToResultSet();

            return finalResuelt;
        }


    }
}