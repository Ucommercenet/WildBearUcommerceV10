using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace WildBearAdventures.API.Product_Definitions
{
    public class CustomProductIndexDefinition : DefaultProductsIndexDefinition
    {

        public CustomProductIndexDefinition()
        {
            //is searchable, is visible via back office
            this.Field(p => p["OriginCountry"], typeof(string)).Facet();

            this.Field(p => p["CoffeeTaste"], typeof(string)).Facet();

            //only is visible via back office
            this.Field(p => p["InternalRating"], typeof(string));

            //Should be hidden?
            this.Field(p => p["__HiddenRating"], typeof(string)).Facet();

        }
    }
}
