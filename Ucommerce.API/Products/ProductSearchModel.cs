using System.Collections.Immutable;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace Ucommerce.API.Products
{
    public class ProductSearchResult
    {
        public ProductSearchModel ProductSearchModel { get; set; }

        public IImmutableDictionary<string, object?> UserDefinedFields { get; set; }

    }
}
