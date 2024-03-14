
using System.Globalization;
using Ucommerce.Extensions.Search.Abstractions.Models.IndexModels;
using Ucommerce.Extensions.Search.Abstractions.Models.SearchModels;

namespace WildBearAdventures.API
{
    public class StartupSandbox : BackgroundService
    {
        private readonly IIndex<ProductSearchModel> _indexProduct;

        public StartupSandbox(IIndex<ProductSearchModel> indexProduct)
        {
            _indexProduct = indexProduct;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            IsIndexUpToData();
            


            throw new NotImplementedException();
        }

        private void IsIndexUpToData()
        {
             //Culture
            
            var culture = new CultureInfo("da-DK");
            
            var someProduct = _indexProduct.AsSearchable(culture).First();

           


            throw new NotImplementedException();
        }
    }
}
