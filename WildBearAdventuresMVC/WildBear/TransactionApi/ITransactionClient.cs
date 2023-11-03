namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public interface ITransactionClient
    {
        Task<string> CreateBasket(string currency, string cultureCode, CancellationToken token);
    }
}