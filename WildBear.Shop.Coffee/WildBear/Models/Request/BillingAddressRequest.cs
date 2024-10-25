namespace WildBear.Shop.Coffee.WildBear.Models.Request
{
    public class BillingAddressRequest
    {
        public required Guid ShoppingCartGuid { get; set; }
        public required string City { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PostalCode { get; set; }
        public string? Line1 { get; set; }
        public required string CountryId { get; set; }
        public required string Email { get; set; }
        public string? State { get; set; }
        public required string MobileNumber { get; set; }
        public string? Attention { get; set; }
        public string? CompanyName { get; set; }

    }
}
