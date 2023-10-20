using UcommerceWildBearDTO;

namespace WildBearAdventuresMVC.Models
{
    public class CategoryViewModel
    {
        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int CategoryProductCount { get; set; }

        public List<ProductDto>? ProductDtos { get; set; }


    }
}
