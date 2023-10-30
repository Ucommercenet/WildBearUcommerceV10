using UcommerceWildBearDTO;

namespace WildBearAdventuresMVC.Models
{
    public class CategoryViewModel
    {
        public string CurrentCategoryName { get; set; }

        public string Description { get; set; }

        public int CategoryProductCount { get; set; }


        //TODO: update to using ProductViewModel
        public List<ProductDto>? ProductDtos { get; set; }


    }
}
