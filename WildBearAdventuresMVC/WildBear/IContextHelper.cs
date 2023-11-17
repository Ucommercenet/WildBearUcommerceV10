﻿namespace WildBearAdventuresMVC.WildBear
{
    public interface IContextHelper
    {
        int GetCurrentCartCount();
        Guid? GetCurrentCartGuid();
        Guid? GetCurrentCategoryGuid();
        Guid? GetCurrentProductGuid();
        void SetCurrentCart(Guid basket, int startingItemCount = 1);
        void SetCurrentCategoryByName(string currentCategoryName);
        void SetCurrentProductByName(string currentProductName);
        int UpdateCurrentCartCount(int updatevalue);
    }
}