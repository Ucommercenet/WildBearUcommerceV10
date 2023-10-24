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

        public Guid? GetCurrentCategory()
        {

            var session = _httpContextAccessor.HttpContext?.Session;


            var categoryGuid = session?.GetString(KEY_CategoryGuid);

            var isGuidFound = Guid.TryParse(categoryGuid, out var result);

            //Test 1
            var testValue = session.GetString("TestKey");

            return (isGuidFound) ? result : null;
        }

        public void SetCurrentCategory(string currentCategoryName)
        {
            var session = _httpContextAccessor.HttpContext?.Session;


            var CategoryGuid = _wildBearApiClient.GetOnlyGuidByName(currentCategoryName, new CancellationToken());
            session?.SetString(KEY_CategoryGuid, CategoryGuid.ToString());

            //Test 1
            var testValue = session.GetString("TestKey");

            //Test 2
            var categoryGuid = session?.GetString(KEY_CategoryGuid);


        }







    }

}

