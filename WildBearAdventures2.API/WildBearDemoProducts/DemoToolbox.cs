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
using static Ucommerce.Web.BackOffice.Constants.FieldIdConstants;

namespace WildBearAdventures.API.WildBearDemoProducts
{
    public class DemoToolbox
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private IIndexer<ProductEntity> _productIndexer;

        public DemoToolbox(UcommerceDbContext ucommerceDbContext, IIndexer<ProductEntity> productIndexer)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _productIndexer = productIndexer;
        }

        /// <summary>
        /// Will both create and Index the newly created products.
        /// </summary>
        public async Task<List<ProductEntity>> CreateCoffeeProducts(CancellationToken cancellationToken)
        {
            var wildCoffeeDefinition = _ucommerceDbContext.Set<ProductDefinitionEntity>().FirstOrDefault(x => x.Name == "WildCoffee");
            if (wildCoffeeDefinition == null) { throw new Exception("WildCoffee ProductDefinitionEntity not found"); }

            var demoProducts = new List<ProductEntity>
            {
                //TODO: Add option to create any number if needed
               await CreateRegularProduct(name: $"DemoCoffee{RandomLetterAndNumber()}", sku:RandomLetterAndNumber(), productDefinition: wildCoffeeDefinition, culture: "da-DK"),
            };
            _ucommerceDbContext.Set<ProductEntity>().AddRange(demoProducts);


            var drinksCategory = _ucommerceDbContext.Set<CategoryEntity>().Where(x => x.Name == "Drinks").First();
            CreateCategoryProductRelation(demoProducts, drinksCategory);

            _ucommerceDbContext.SaveChanges();

            //TODO: bug in the indexer! For now will only work if product are in a category
            await _productIndexer.Index(demoProducts.ToImmutableList(), cancellationToken);
            return demoProducts;
        }


        /// <summary>
        /// Creates a RegularProduct(aka non variant product)
        /// Note: name, DisplayName definition, culture are required        
        /// </summary>
        public async Task<ProductEntity> CreateRegularProduct(string name, string sku, ProductDefinitionEntity productDefinition, string culture, string? shortDescription = null, decimal? price = null)
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
                Definition = productDefinition,
                DisplayOnSite = true,
                ProductDescriptions = new List<ProductDescriptionEntity> { new ProductDescriptionEntity() { DisplayName = name, ShortDescription = shortDescription, CultureCode = culture } }
            };


            return product;
        }

        public void CreateCategoryProductRelation(List<ProductEntity> demoProducts, CategoryEntity category)
        {
            foreach (var product in demoProducts)
            {
                var categoryProductRelation = new CategoryProductRelationEntity()
                {
                    Category = category,
                    Product = product
                };
                _ucommerceDbContext.Set<CategoryProductRelationEntity>().Add(categoryProductRelation);
            }

        }

        /// <summary>
        /// Will use the Default Category Definition
        /// </summary>        
        public void CreateCategory(string categoryName)
        {
            //Example of how to use Include
            //var existingCategoryEntity = _ucommerceDbContext.Set<CategoryEntity>()
            //    .Where(x => x.Name == "Software").Include(x => x.Definition).First();


            var defaultCategoryDefinition = _ucommerceDbContext.Set<DefinitionEntity>()
                .Where(x => x.Name == "Default Category Definition").First();

            var defaultCatalog = _ucommerceDbContext.Set<CatalogEntity>()
              .Where(x => x.Deleted == false).First();

            var category = new CategoryEntity() { Name = categoryName, Definition = defaultCategoryDefinition, Catalog = defaultCatalog };


            _ucommerceDbContext.Set<CategoryEntity>().Add(category);
            _ucommerceDbContext.SaveChanges();
        }

        public void CreateProductDefinition(string definitionName)
        {
            var WildCoffeeProductDefinition = new ProductDefinitionEntity()
            {
                Name = definitionName,
                Description = "Definition for Coffee type products",
                Deleted = false,
            };

            _ucommerceDbContext.Add(WildCoffeeProductDefinition);
            _ucommerceDbContext.SaveChanges();

        }

        public void AddShortTextFieldToProductDefinition(ICollection<ProductDefinitionFieldEntity> definitionFields, string nameOfNewField)
        {
            var shortTextDataType = _ucommerceDbContext.Set<DataTypeEntity>().First(x => x.DefinitionName == "ShortText");

            definitionFields.Add(CreateProductDefinitionField(shortTextDataType, nameOfNewField, false, false));

            _ucommerceDbContext.Add(definitionFields);
            _ucommerceDbContext.SaveChanges();


        }



        private ProductDefinitionFieldEntity CreateProductDefinitionField(DataTypeEntity dataType, string name, bool isMultilingual, bool isVariantProperty)
        {
            return new ProductDefinitionFieldEntity
            {
                Name = name,
                Deleted = false,
                Multilingual = isMultilingual,
                DisplayOnSite = true,
                RenderInEditor = true,
                IsVariantProperty = isVariantProperty,
                DataType = dataType
            };
        }
        private string RandomLetterAndNumber()
        {
            var random = new Random();

            // Generate a random letter (A-Z)
            char letter = (char)('A' + random.Next(0, 26));

            // Generate a random number (0-9)
            int number = random.Next(0, 10);

            // Concatenate the letter and number and return as a string
            return letter.ToString() + number.ToString();
        }
    }
}
