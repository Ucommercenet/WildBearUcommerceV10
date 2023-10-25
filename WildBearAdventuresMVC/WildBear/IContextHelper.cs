namespace WildBearAdventuresMVC.WildBear
{
    public interface IContextHelper
    {
        Guid? GetCurrentCategoryGuid();
        Guid? GetCurrentProductGuid();
        void SetCurrentCategoryByName(string currentCategoryName);
        void SetCurrentProductByName(string currentProductName);
    }
}