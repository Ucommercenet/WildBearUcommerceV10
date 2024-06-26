﻿using WildBearAdventures.MVC.WildBear.Models.DTOs;

namespace WildBearAdventures.MVC.ViewModels
{
    public class CategoryViewModel
    {
        public string CurrentCategoryName { get; set; }

        public string Description { get; set; }

        public int CategoryProductCount { get; set; }

        public List<ProductDto>? ProductDtos { get; set; }

        public string ImageUrl { get; set; }


    }
}
