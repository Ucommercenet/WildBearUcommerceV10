namespace WildBearAdventuresMVC.WildBear.TransactionApi
{
    public interface ITransactionClient
    {
        Task<Guid> CreateBasket(string currency, string cultureCode, CancellationToken token);
    }
}