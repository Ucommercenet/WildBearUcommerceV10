using WildBearAdventuresMVC.WildBear.TransactionApi.Models;

namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public interface IStoreAuthentication
    {
        StoreAuthenticationModel AuthenticationModel { get; }
    }
}