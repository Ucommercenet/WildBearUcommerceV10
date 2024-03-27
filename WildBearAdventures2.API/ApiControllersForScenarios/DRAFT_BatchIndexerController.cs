using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Persistence.Entities;

namespace WildBearAdventures.API.ApiControllersForScenarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class DRAFT_BatchIndexerController : ControllerBase
    {
        private readonly IEnumerable<IIndexer<ProductEntity>> _ProductIndexers;
        private readonly IIndex<ProductSearchModel> _indexProduct;

        public DRAFT_BatchIndexerController(IEnumerable<IIndexer<ProductEntity>> productIndexer)
        {
            _ProductIndexers = productIndexer;
        }


        [HttpPost("BatchIndexer")]
        public void BatchIndexer()
        {

            //foreach (var indexer in _ProductIndexers)
            //{
            //    indexer.Index();

            //}

        }



    }
}
