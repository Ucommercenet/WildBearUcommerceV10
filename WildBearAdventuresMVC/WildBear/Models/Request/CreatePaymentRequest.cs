namespace WildBearAdventures.MVC.WildBear.Models.Request
{
    public class CreatePaymentRequest
    {
        public required Guid ShoppingCartGuid { get; set; }

        public required string CultureCode { get; set; }

        public required Guid PaymentMethodGuid { get; set; }

        public required Guid PriceGroupGuid { get; set; }
        
        



    }
}
