namespace WildBearAdventuresMVC.WildBear.Models.DTOs
{
    public class ProductDto
    {
        public bool allowOrdering { get; set; }
        public string[] CategoryIds { get; set; }
        public string DisplayName { get; set; }
        public string LongDescription { get; set; }
        public string ParentProductId { get; set; }
        public string[] PriceGroupIds { get; set; }
        public Pricesincltax PricesInclTax { get; set; }
        public object PrimaryImageMediaId { get; set; }
        public object PrimaryImageUrl { get; set; }
        public string ProductDefinitionId { get; set; }
        public int ProductType { get; set; }
        public object[] RelatedProductIds { get; set; }
        public string ShortDescription { get; set; }
        public string Sku { get; set; }
        public Taxes Taxes { get; set; }
        public object ThumbnailImageMediaId { get; set; }
        public object ThumbnailImageUrl { get; set; }

        //TODO: Needs to handle collection of UnitPrices but it works for now 
        public Unitprices UnitPrices { get; set; }
        public string[] VariantIds { get; set; }
        public string VariantSku { get; set; }
        public Variantsproperties VariantsProperties { get; set; }
        public DateTime __lastIndexed { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Pricesincltax
    {
        public decimal EUR15pct { get; set; }
    }

    public class Taxes
    {
        public decimal EUR15pct { get; set; }
    }

    public class Unitprices
    {
        public decimal EUR15pct { get; set; }
    }

    public class Variantsproperties
    {
        //Placeholder for Variants properties
    }




}
