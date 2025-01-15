using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Dynamic.Core;
using Ucommerce.Extensions.Search.Abstractions;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;

namespace Ucommerce.API.ApiControllers.Sandbox
{
    public class ProductUtilities
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private IIndexer<ProductEntity> _productIndexer;

        public ProductUtilities(UcommerceDbContext ucommerceDbContext, IIndexer<ProductEntity> productIndexer)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productIndexer = productIndexer;
        }


        /// <summary>
        /// Creates a RegularProduct(aka non variant product)
        /// Note: name, DisplayName definition, culture are required        
        /// </summary>
        public ProductEntity CreateRegularProduct(string name, string sku, Guid productDefinitionGuid, string culture, string? shortDescription = null, decimal? price = null)
        {
            var startingPrice = new PriceEntity() { };
            var priceCollection = new List<PriceEntity>
            {
                startingPrice
            };

            var product = new ProductEntity()
            {
                Name = name,
                Sku = sku,
                DefinitionGuid = productDefinitionGuid,
                DisplayOnSite = true,
                ProductDescriptions = new List<ProductDescriptionEntity> { new ProductDescriptionEntity() { DisplayName = name, ShortDescription = shortDescription, CultureCode = culture } }
            };


            return product;
        }

        public void AddProductsToCategory(List<ProductEntity> productList, CategoryEntity category)
        {
            foreach (var product in productList)
            {
                var categoryProductRelation = new CategoryProductRelationEntity()
                {
                    Category = category,
                    Product = product
                };

            }

        }
        public CategoryEntity CreateCategory(string categoryName)
        {
            var defaultCategoryDefinition = _ucommerceDbContext.Set<DefinitionEntity>()
                .Where(x => x.Name == "Default Category Definition").First();

            var defaultCatalog = _ucommerceDbContext.Set<CatalogEntity>()
              .Where(x => x.Deleted == false).First();

            var category = new CategoryEntity() { Name = categoryName, DefinitionGuid = defaultCategoryDefinition.Guid, Catalog = defaultCatalog, DisplayOnSite = true };

           
            return category;
        }

        public ProductDefinitionEntity CreateProductDefinition(string definitionName)
        {
            var WildCoffeeProductDefinition = new ProductDefinitionEntity()
            {
                Name = definitionName,
                Description = "Definition for any type of products",
                Deleted = false,
                ProductDefinitionFields = []
            };            

            return WildCoffeeProductDefinition;
        }

    }
}
