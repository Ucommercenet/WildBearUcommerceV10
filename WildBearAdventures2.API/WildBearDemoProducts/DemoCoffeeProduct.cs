namespace WildBearAdventures.API.WildBearDemoProducts
{
    public class DemoCoffeeProduct
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }

        public DemoCoffeeProduct(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

    }
}
