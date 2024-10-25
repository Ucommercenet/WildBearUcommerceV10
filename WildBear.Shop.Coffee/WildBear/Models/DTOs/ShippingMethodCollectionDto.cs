namespace WildBear.Shop.Coffee.WildBear.Models.DTOs
{
    public class ShippingMethodCollectionDto
    {
        public object? PagingToken { get; set; }
        public required List<ShippingMethod> ShippingMethods { get; set; }

        public class ShippingMethod
        {
            public string? Description { get; set; }
            public string? DisplayName { get; set; }
            public required string Id { get; set; }
            public string? ImageUrl { get; set; }
            public required string Name { get; set; }
            public required Price Price { get; set; }
        }

        public class Price
        {
            public decimal Amount { get; set; }
            public required string Currency { get; set; }
        }
    }
}
