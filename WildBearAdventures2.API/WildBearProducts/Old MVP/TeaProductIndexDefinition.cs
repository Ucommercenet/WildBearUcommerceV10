using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace WildBearAdventures.API.WildBearProducts
{
    public class TeaProductIndexDefinition : DefaultProductsIndexDefinition
    {

        public TeaProductIndexDefinition()
        {
            this.Field(p => p["Tea Flavor Profile"], typeof(string));
        }


    }
}
