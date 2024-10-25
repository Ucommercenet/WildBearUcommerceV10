namespace WildBear.Shop.Coffee.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public Guid ProductGuid { get; set; }

        public int Quantity { get; set; } = 0;

        public string ImageUrl { get; set; }




    }

}