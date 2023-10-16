using Microsoft.AspNetCore.Mvc;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace HeadlessProjectv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IIndex<CategorySearchModel> _indexCategory;



        public CategoryController(IIndex<CategorySearchModel> indexCategory)
        {
            _indexCategory = indexCategory;

        }



    }
}
