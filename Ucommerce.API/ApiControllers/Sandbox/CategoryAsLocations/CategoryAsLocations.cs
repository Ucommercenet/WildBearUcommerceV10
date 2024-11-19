using Microsoft.AspNetCore.Mvc;
using Ucommerce.Web.Infrastructure.Persistence.Entities.Definitions;
using Ucommerce.Web.Infrastructure.Persistence;
using Ucommerce.Web.Infrastructure.Persistence.Entities;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;
using System.Globalization;
using Ucommerce.Web.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Ucommerce.Web.BackOffice.Constants;
using Ucommerce.Web.BackOffice.Mappers;
using Ucommerce.Web.BackOffice.Models.ViewModels.DefinitionData;
using Ucommerce.Web.BackOffice.Pipelines.Category.GetCategory;
using Ucommerce.Web.Core.Pipelines.Definition.GetDefinition;
using Ucommerce.Web.Infrastructure.Security.Exceptions;
using Elastic.Clients.Elasticsearch;
using Ucommerce.Web.Infrastructure.Pipelines;

namespace Ucommerce.API.ApiControllers.Sandbox.CategoryAsLocations
{


    //Remark: Change ControllerBase to BackOfficeControllerBase if Auth is needed.
    public class CategoryAsLocations : ControllerBase
    {
        private readonly UcommerceDbContext _ucommerceDbContext;
        private readonly IDataMapper<CategoryEntity, DefinitionEntity> _dataMapper;
        private readonly IPipeline<GetCategoryInput, GetCategoryOutput> _getCategoryPipeline;
        private readonly IPipeline<GetDefinitionInput, GetDefinitionOutput> _getDefinitionPipeline;


        public CategoryAsLocations(UcommerceDbContext ucommerceDbContext, IDataMapper<CategoryEntity, DefinitionEntity> dataMapper, IPipeline<GetCategoryInput, GetCategoryOutput> getCategoryPipeline, IPipeline<GetDefinitionInput, GetDefinitionOutput> getDefinitionPipeline)
        {
            _ucommerceDbContext = ucommerceDbContext;
            _dataMapper = dataMapper;
            _getCategoryPipeline = getCategoryPipeline;
            _getDefinitionPipeline = getDefinitionPipeline;
        }

        [HttpPost("CreateNewDefinitionForCategory")]
        public IActionResult CreateNewDefinitionForCategory(string categoryDefinitionName = "CategoryWithLocationInfo")
        {

            //Part 1 Creating the newCategoryDefinition
            var newCategoryDefinition = new DefinitionEntity()
            {
                Name = categoryDefinitionName,
                Description = "category Definition with locationInfo as a fields",
                Deleted = false,
                DefinitionTypeId = 1, // 1 is the id for CategoryDefinition
                DefinitionFields = new List<DefinitionFieldEntity>()
            };


            //Part 2 Creating the field on the newCategoryDefinition
            var numberDataType = _ucommerceDbContext.Set<DataTypeEntity>()
             .FirstOrDefault(x => x.DefinitionName == "Number") ?? throw new Exception("DataType not found");


            var latitudeFieldEntity = CreateDefinitionField(numberDataType, "LocationInfo_Latitude");
            var longitudeFieldEntity = CreateDefinitionField(numberDataType, "LocationInfo_Longitude");


            //Part 3 adding the fields to the newCategoryDefinition
            newCategoryDefinition.DefinitionFields.Add(latitudeFieldEntity);
            newCategoryDefinition.DefinitionFields.Add(longitudeFieldEntity);

            _ucommerceDbContext.Add(newCategoryDefinition);
            _ucommerceDbContext.SaveChanges();

            return Ok();
        }


        [HttpGet("TEMP_GetCategoryLocationInfo")]
        public async Task<IActionResult> GetCategoryLocationInfo(string searchName)
        {



            var searchResult = _ucommerceDbContext.Set<CategoryEntity>()
                .Where(x => x.Name == searchName)
                .Include(x => x.PropertiesRaw)
                .FirstOrDefault();

            var boo = searchResult.PropertiesRaw.ToList();

            //var searchResult2 = _ucommerceDbContext.Set<DefinitionEntity>()
            //     .Where(x => x. == searchName)
            //     .FirstOrDefault();



            //var culture = new CultureInfo("da-DK");



            //var indexSearch = await _indexCategory.AsSearchable(culture)
            //   .Where(p => p.Name == searchName)
            //   .ToResultSet();

            //var result = indexSearch.SingleOrDefault()?.GetUserDefinedFields();


            var data = ImmutableDictionary<string, DefinitionDataViewModelBase>.Empty;

            var debugTemp = _dataMapper;



            return Ok();

        }


        [HttpGet("GetCategoryLocationInfoViaPipelines")]
        public async Task<ActionResult<ImmutableDictionary<string, DefinitionDataViewModelBase>>> GetCategoryLocationInfoViaPipelines(string categoryGuid, string cultureCode, CancellationToken token)
        {
            try
            {
                var cultureInfo = new CultureInfo(cultureCode);
                var input = new GetCategoryInput(Guid.Parse(categoryGuid)) { HydrateCategoryDescriptions = true };
                var result = await _getCategoryPipeline.Execute(input, token);
                var output = result.EnsureSuccess();
                var category = output.Category;

                if (category is null)
                {
                    var msg = "Requested category: {CategoryGuid} could not be found";
                    return NotFound(msg);
                }

                var definitionInput = new GetDefinitionInput(category.DefinitionGuid, cultureInfo);
                var definitionResult = await _getDefinitionPipeline.Execute(definitionInput, token);
                var definitionOutput = definitionResult.EnsureSuccess();

                if (definitionOutput.Definition is not DefinitionEntity definition)
                {
                    return NotFound($"Category definition with guid {category.DefinitionGuid} was not found, or of the wrong type.");
                }

                IImmutableDictionary<string, DefinitionDataViewModelBase> data = ImmutableDictionary<string, DefinitionDataViewModelBase>.Empty;
                data = await _dataMapper.AddMeta(data, category, definition, NodeTypeConstants.PRODUCT_CATEGORY, token: token);
                data = await _dataMapper.AddProperties(data, category, definition, cultureInfo, token);

                return data.ToImmutableDictionary();
            }
            catch (Exception e) when (e.FindException<MissingRoleException>() != null)
            {
                //TODO: log or message the user                
                return Forbid();
            }


        }





        ///// Get properties for a given Category guid and culture code.
        //[HttpGet("{categoryGuid}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IImmutableDictionary<string, DefinitionDataViewModelBase>))]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<ImmutableDictionary<string, DefinitionDataViewModelBase>>> GetCategoryDetails(
        //    [FromRoute] string categoryGuid,
        //    [FromQuery][Required][MinLength(5)] string cultureCode,
        //    CancellationToken token)
        //{
        //    try
        //    {
        //        var cultureInfo = new CultureInfo(cultureCode);
        //        var input = new GetCategoryInput(Guid.Parse(categoryGuid)) { HydrateCategoryDescriptions = true };
        //        var result = await _getCategoryPipeline.Execute(input, token);
        //        var output = result.EnsureSuccess();
        //        var category = output.Category;

        //        if (category is null)
        //        {
        //            var msg = "Requested category: {CategoryGuid} could not be found";
        //            return NotFound(msg);
        //        }

        //        var definitionInput = new GetDefinitionInput(category.DefinitionGuid, cultureInfo);
        //        var definitionResult = await _getDefinitionPipeline.Execute(definitionInput, token);
        //        var definitionOutput = definitionResult.EnsureSuccess();

        //        if (definitionOutput.Definition is not DefinitionEntity definition)
        //        {
        //            return NotFound($"Category definition with guid {category.DefinitionGuid} was not found, or of the wrong type.");
        //        }

        //        IImmutableDictionary<string, DefinitionDataViewModelBase> data = ImmutableDictionary<string, DefinitionDataViewModelBase>.Empty;
        //        data = await _dataMapper.AddMeta(data, category, definition, NodeTypeConstants.PRODUCT_CATEGORY, token: token);
        //        data = await _dataMapper.AddProperties(data, category, definition, cultureInfo, token);

        //        return data.ToImmutableDictionary();
        //    }
        //    catch (Exception e) when (e.FindException<MissingRoleException>() != null)
        //    {
        //        //TODO: log or message the user                
        //        return Forbid();
        //    }


        //}




        private static DefinitionFieldEntity CreateDefinitionField(DataTypeEntity numberDataType, string name)
        {
            var definitionFieldEntity = new DefinitionFieldEntity
            {
                Name = name,
                Deleted = false,
                Multilingual = false,
                DisplayOnSite = false, // false = not in the index change this if needed
                RenderInEditor = true, //Remark: This can be changed to false if the back office user is not allowed to see the location info                
                DataType = numberDataType,
            };

            return definitionFieldEntity;
        }
    }
}
