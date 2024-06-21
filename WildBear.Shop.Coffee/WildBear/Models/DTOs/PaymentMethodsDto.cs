namespace WildBear.Shop.Coffee.WildBear.Models.DTOs
{

    public class PaymentMethodsDto
    {
        public Paymentmethod[] PaymentMethods { get; set; }
    }

    public class Paymentmethod
    {
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public float FeePercent { get; set; }
        public Fee[] Fees { get; set; }
        public string Id { get; set; }
        public object ImageId { get; set; }
        public object ImageUrl { get; set; }
        public string Name { get; set; }
        public Paymentmethodproperty[] PaymentMethodProperties { get; set; }
    }

    public class Fee
    {
        public float Amount { get; set; }
        public string PriceGroupId { get; set; }
    }

    public class Paymentmethodproperty
    {
        public string CultureCode { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
    }



}
