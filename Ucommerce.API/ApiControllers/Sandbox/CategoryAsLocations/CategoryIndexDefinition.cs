using Microsoft.Extensions.Options;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.DefaultDefinitions;
using Ucommerce.Extensions.Search.Abstractions.Extensions;

namespace Ucommerce.API.ApiControllers.Sandbox.CategoryAsLocations
{
    public class CategoryIndexDefinition : DefaultCategoriesIndexDefinition
    {

        public CategoryIndexDefinition()
        {
            this.Field(p => p["LocationInfo_Latitude"], typeof(decimal))/*.Facet()*/;
            this.Field(p => p["LocationInfo_Longitude"], typeof(decimal))/*.Facet()*/;
        }

    }
}
