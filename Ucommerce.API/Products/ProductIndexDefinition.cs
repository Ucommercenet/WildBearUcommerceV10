using Microsoft.Extensions.Options;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace Ucommerce.API.WildBearDemoProducts
{
    public class ProductIndexDefinition : DefaultProductsIndexDefinition
    {
        public ProductIndexDefinition(IOptions<SearchOptions> searchOptions) : base(searchOptions)
        {
            this.Field(p => p["Origin Country"], typeof(string)).Facet();
            this.Field(p => p["Taste and flavor"], typeof(string)).Facet();




        }
    }
}
