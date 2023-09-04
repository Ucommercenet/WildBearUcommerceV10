using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Core.Models;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PitfallsController : ControllerBase
    {
        //WARNING The purpose of this class is to showcase common pitfalls in Ucommerce -- Some code here will not work!

        //TODO: Make good description why this is a bad idea TLDR: IIndex needs a type like IIndex<Product> 

        private readonly IIndex _index;

        public PitfallsController(IIndex index)
        {
            _index = index;
        }

        [HttpGet("GetProductNameForA001")]
        public string GetProductNameForA001(CancellationToken token)
        {
            var language = new Language()
            {
                Name = "Danish",
                Culture = new CultureInfo("da-DK")
            };

            var indexSearch = _index.AsSearchable<ProductSearchModel>(language.Culture);

            var productResultSet = indexSearch.Where(x => x.Sku == "A001")
                .ToResultSet(token).Result;

            var result = productResultSet.FirstOrDefault()?.Name;

            return result ??= "No product found";
        }
    }
}