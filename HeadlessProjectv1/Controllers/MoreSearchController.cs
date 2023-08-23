using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoreSearchController : ControllerBase
    {
        private readonly IIndex _index;

        public MoreSearchController(IIndex index)
        {
            _index = index;
        }

        //[HttpGet("GetYesYes")]
        //public string GetYesYes()
        //{
        //    return "yes yes it works";
        //}

        /*Note:
        CatalogSearchModel is not working
        StoreSearchModel is not working

        */

        //TODO: Should method be async?
        //DOES NOT WORK CatalogSearchModel is broken
        [HttpGet("GetDefaultCatalog")]
        public CatalogSearchModel? GetDefaultCatalog()
        {
            var culture = new CultureInfo("da-DK");

            var searchable = _index.AsSearchable<CatalogSearchModel>(culture);

            var storeSearchModel = searchable.FirstOrDefault(new CancellationToken()).Result;

            return storeSearchModel;
        }
    }
}