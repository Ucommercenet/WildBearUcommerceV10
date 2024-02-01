namespace WildBearAdventures.MVC.WildBear.Models.Request
{
    public class ShippingInformationRequest
    {
        public required Guid ShoppingCartGuid {  get; set; }
        public required string PriceGroupId { get; set; }
        public required string ShippingMethodId { get; set; }
        public required string CultureCode { get; set; }
        public required Address ShippingAddress { get; set; }

        public class Address
        {
            public string? City { get; set; }
            public string? CompanyName { get; set; }
            public string? CountryId { get; set; }
            public string? Email { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Line1 { get; set; }
            public string? Line2 { get; set; }
            public string? MobileNumber { get; set; }
            public string? PhoneNumber { get; set; }
            public string? PostalCode { get; set; }
            public string? State { get; set; }
        }
    }

}
