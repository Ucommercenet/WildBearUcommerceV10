namespace WildBear.Shop.Coffee.WildBear.Context
{
    public interface IContextHelper
    {
        int GetCurrentCartCount();
        Guid? GetCurrentCartGuid();
        Guid? GetCurrentCategoryGuid();

        void SetCurrentCart(Guid basket);
        void SetCurrentCategoryByName(string currentCategoryName);

        void UpdateMiniCartCount(int updatevalue);
    }
}