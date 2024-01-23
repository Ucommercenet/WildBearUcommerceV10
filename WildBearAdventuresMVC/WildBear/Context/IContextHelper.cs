namespace WildBearAdventures.MVC.WildBear.Context
{
    public interface IContextHelper
    {
        int GetCurrentCartCount();
        Guid? GetCurrentCartGuid();
        Guid? GetCurrentCategoryGuid();
        Guid? GetCurrentProductGuid();
        void SetCurrentCart(Guid basket);
        void SetCurrentCategoryByName(string currentCategoryName);
        void SetCurrentProductByName(string currentProductName);
        void UpdateCurrentShoppingCartCount(int updatevalue);
    }
}