using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace WildBearAdventures.API.Product_Definitions
{
    public class CustomProductIndexDefinition : DefaultProductsIndexDefinition
    {

        public CustomProductIndexDefinition() : base()
        {
            this.Field(p => p["OriginCountry"], typeof(string));
            
            this.Field(p => p["Kan downloades"], typeof(bool));

        }
    }
}
