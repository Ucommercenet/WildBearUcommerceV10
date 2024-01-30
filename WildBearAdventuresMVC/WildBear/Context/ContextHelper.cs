using WildBearAdventures.MVC.WildBear.TransactionApi;

namespace WildBearAdventures.MVC.WildBear.Context
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
        private const string KEY_BasketGuid = "CurrentBasketGuid";
        private const string KEY_BasketCount = "CurrentBasketCount";



        public Guid? GetCurrentCategoryGuid()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var categoryGuid = session?.GetString(KEY_CategoryGuid);
            var isGuidFound = Guid.TryParse(categoryGuid, out var result);

            return isGuidFound ? result : null;
        }

        public void SetCurrentCategoryByName(string currentCategoryName)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            var CategoryGuid = _wildBearApiClient.GetOnlyCategoryGuidByName(currentCategoryName, new CancellationToken());
            session?.SetString(KEY_CategoryGuid, CategoryGuid.ToString());


            
        }


        /// <summary>
        /// Sets the currentBasketGuid
        /// </summary>
        /// <param name="basket"></param>
        public void SetCurrentCart(Guid basket)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(KEY_BasketGuid, basket.ToString());

        }

        /// <summary>
        /// Remark if no value found, just retrun the updateValue
        /// </summary>
        /// <param name="updateValue"></param>
        /// <returns>The value after the update</returns>
        /// <remarks>Optimize: in some edge cases the cart count could be negative</remarks>
        public void UpdateMiniCartCount(int updateValue)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var oldValue = session?.GetInt32(KEY_BasketCount);

            int newValue;
            if (oldValue.HasValue)
            {
                newValue = oldValue.Value + updateValue;
            }
            else
            {
                newValue = updateValue;
            }

            session?.SetInt32(KEY_BasketCount, newValue);
        }

        public int GetCurrentCartCount()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var count = session?.GetInt32(KEY_BasketCount);
            //Remark if no value can be found just retrun 0
            return count.HasValue ? count.Value : 0;
        }



        public Guid? GetCurrentCartGuid()

        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var currentBasketGuid = session?.GetString(KEY_BasketGuid);
            var isGuidFound = Guid.TryParse(currentBasketGuid, out var result);

            return isGuidFound ? result : null;


        }



    }

}

