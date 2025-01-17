using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Ucommerce.Web.BackOffice.Pipelines.Category.CreateCategory;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Pipelines;
using static Ucommerce.Web.Core.Constants.FieldIdConstants;

namespace Ucommerce.API.ApiControllers.Sandbox.InitializeExamples
{
    public class InitializeProductsWithPipelines : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private IPipeline<CreateCategoryInput, CreateCategoryOutput> _createCategoryPipeline;


        public InitializeProductsWithPipelines(IPipeline<CreateCategoryInput, CreateCategoryOutput> createCategoryPipeline, UcommerceDbContext ucommerceDbContext)
        {
            _createCategoryPipeline = createCategoryPipeline;
            _ucommerceDbContext = ucommerceDbContext;
        }

        [HttpPost("WORKINPROGRESS_InitializeProductsWithPipelinesSequence")]
        public async Task<ActionResult> WORKINPROGRESS_InitializeProductsWithPipelinesSequence(CancellationToken cancellationToken)
        {
            //Names for the theme of this Sequence
            var productNameSeed = "PipelineCoffee1";
            var categoryName = "PipelineDrinks1";
            var productDefinitionName = $"{productNameSeed} And other hot beverages";
            var ProductDefinitionFieldName = "Taste";
            var culture = "da-DK";



            //var boo = CreateCategoryInput(Guid DefinitionGuid, string Name, Guid CatalogId, int SortOrder, CultureInfo CultureInfo)


            //THIS is not a Category Definition!!?
            ////TODO: Ooops we need to Find the Category DefinitionEntity
            var categoryDefinitionEntity = _ucommerceDbContext.Set<DefinitionFieldEntity>().FirstOrDefault(x => x.Name != "TODO");
            //  .FirstOrDefault(x => x.Name == productDefinitionName)?.Guid) ?? throw new Exception("DefinitionEntity not found");


            var defaultCategoryDefinition = _ucommerceDbContext.Set<DefinitionEntity>()
              .Where(x => x.Name == "Default Category Definition").First();

            var catalogGuid = _ucommerceDbContext.Set<CatalogEntity>().FirstOrDefault(x => x.Name == "Ucommerce")?.Guid;
            if (catalogGuid == null)
            {
                //If none is name Ucommerce just find the first catalog
                catalogGuid = _ucommerceDbContext.Set<CatalogEntity>().First().Guid;
            }



            // Error Message User is missing role of type: Ucommerce.Web.Infrastructure.Persistence.Entities.Roles.CreateCatalogRoleEntity
            // This needs to be called by the WildBear Client not "just" swagger.

            var pipeLineInput = new CreateCategoryInput(
                DefinitionGuid: categoryDefinitionEntity.Guid,
                Name: categoryName,
                CatalogId: catalogGuid.Value,
                SortOrder: 0,
                CultureInfo: new CultureInfo(culture));

            var result = await _createCategoryPipeline.Execute(pipeLineInput, cancellationToken);

            result.EnsureSuccess();

            return Ok();
        }



        //var output = result.EnsureSuccess();




        // call the CreateNewCategoryPipeline
        // call the CreateNewProductDefinitionPipeline
        // call the CreateNewProductPipeline
        // call the AddProductToCategoryPipeline
        // call the AddShortTextFieldToProductDefinitionPipeline






    }
}
