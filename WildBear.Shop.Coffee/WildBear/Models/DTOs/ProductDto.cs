namespace WildBear.Shop.Coffee.WildBear.Models.DTOs
{

    public class ProductDto
    {
        public bool AllowOrdering { get; set; }
        public string[] categoryIds { get; set; }
        public string displayName { get; set; }
        public string longDescription { get; set; }
        public string ParentProductId { get; set; }
        public string[] PriceGroupIds { get; set; }

        public string primaryImageUrl { get; set; }
        public string productDefinitionId { get; set; }
        public int productType { get; set; }
        public object[] relatedProductIds { get; set; }
        public string ShortDescription { get; set; }
        public string Sku { get; set; }


        public object[] variantIds { get; set; }
        public string VariantSku { get; set; }
        public DateTime __lastIndexed { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public IDictionary<string, decimal> UnitPrices { get; init; } = new Dictionary<string, decimal>();
    }






}
