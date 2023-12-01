using WildBearAdventuresMVC.Models.WildBearCoffee;

namespace WildBearAdventuresMVC.Models
{
    public partial class CoffeeViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Availability { get; set; }
        public string? ImageUrl { get; set; }

        public RoastLevel RoastLevel { get; set; } = RoastLevel.WildAndWellDone;


    }

}
