using WildBearAdventuresMVC.WildBear.Interfaces;

namespace WildBearAdventuresMVC.WildBear
{
    public class ContextHelper : IContextHelper
    {

        private readonly IWildBearApiClient _wildBearApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextHelper(IWildBearApiClient wildBearApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _wildBearApiClient = wildBearApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private const string KEY_CategoryGuid = "CurrentCategoryGuid";
        private const string KEY_ProductGuid = "CurrentProductGuid";

        public Guid? GetCurrentCategoryGuid()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var categoryGuid = session?.GetString(KEY_CategoryGuid);
            var isGuidFound = Guid.TryParse(categoryGuid, out var result);

            return (isGuidFound) ? result : null;
        }

        public void SetCurrentCategoryByName(string currentCategoryName)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            var CategoryGuid = _wildBearApiClient.GetOnlyCategoryGuidByName(currentCategoryName, new CancellationToken());
            session?.SetString(KEY_CategoryGuid, CategoryGuid.ToString());
        }

        public Guid? GetCurrentProductGuid()

        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var productGuid = session?.GetString(KEY_ProductGuid);
            var isGuidFound = Guid.TryParse(productGuid, out var result);

            return (isGuidFound) ? result : null;

        }

        public void SetCurrentProductByName(string currentProductName)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            //only get id(Guid)
            var ProductGuid = _wildBearApiClient.GetSingleProductByName(currentProductName, new CancellationToken()).Id;
            session?.SetString(KEY_ProductGuid, ProductGuid.ToString());
        }






    }

}

