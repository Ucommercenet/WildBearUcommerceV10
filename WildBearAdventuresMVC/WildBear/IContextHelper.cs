namespace WildBearAdventuresMVC.WildBear
{
    public interface IContextHelper
    {
        Guid? GetCurrentCategory();
        void SetCurrentCategory(string currentCategoryName);
    }
}