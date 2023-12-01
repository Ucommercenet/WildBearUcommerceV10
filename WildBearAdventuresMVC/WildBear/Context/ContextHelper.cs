using WildBearAdventuresMVC.WildBear.Interfaces;

namespace WildBearAdventuresMVC.WildBear
{
    public class ContextHelper : IContextHelper
    {

        private readonly IStoreApiClient _wildBearApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContextHelper(IStoreApiClient wildBearApiClient, IHttpContextAccessor httpContextAccessor)
        {
            _wildBearApiClient = wildBearApiClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private const string KEY_CategoryGuid = "CurrentCategoryGuid";
        private const string KEY_ProductGuid = "CurrentProductGuid";
        private const string KEY_BasketGuid = "CurrentBasketGuid";
        private const string KEY_BasketCount = "CurrentBasketCount";



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

        /// <summary>
        /// Sets the currentBasketGuid and updates the CurrentBasketCount
        /// </summary>
        /// <param name="basket"></param>
        public void SetCurrentCart(Guid basket, int startingItemCount = 1)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(KEY_BasketGuid, basket.ToString());
            session?.SetInt32(KEY_BasketCount, startingItemCount);
        }

        /// <summary>
        /// Remark if no value found, just retrun the updateValue
        /// </summary>
        /// <param name="updateValue"></param>
        /// <returns>The value after the update</returns>
        /// <remarks>Optimize: in some edge cases the cart count could be negative</remarks>
        public int UpdateCurrentShoppingCartCount(int updateValue)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var oldValue = session?.GetInt32(KEY_BasketCount);

            return (oldValue.HasValue) ? oldValue.Value + updateValue : updateValue;

        }

        public int GetCurrentCartCount()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var count = session?.GetInt32(KEY_BasketCount);
            //Remark if no value can be found just retrun 0
            return (count.HasValue) ? count.Value : 0;
        }



        public Guid? GetCurrentCartGuid()

        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var currentBasketGuid = session?.GetString(KEY_BasketGuid);
            var isGuidFound = Guid.TryParse(currentBasketGuid, out var result);

            return (isGuidFound) ? result : null;


        }



    }

}

