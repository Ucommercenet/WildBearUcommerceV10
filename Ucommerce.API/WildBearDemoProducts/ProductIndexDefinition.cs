using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace Ucommerce.API.WildBearDemoProducts
{
    public class ProductIndexDefinition : DefaultProductsIndexDefinition
    {

        public ProductIndexDefinition()
        {
            //is searchable, is visible via back office
            this.Field(p => p["OriginCountry"], typeof(string)).Facet();
            this.Field(p => p["CoffeeAroma"], typeof(string)).Facet();
        }
    }
}
