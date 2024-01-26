namespace WildBearAdventures.MVC.WildBear.Models.Request
{
    public class CreatePaymentRequest
    {
        public required Guid ShoppingCartId { get; set; }

        public required string CultureCode { get; set; }

        public required Guid PaymentMethodId { get; set; }

        public required Guid PriceGroupGuid { get; set; }
        
        



    }
}
