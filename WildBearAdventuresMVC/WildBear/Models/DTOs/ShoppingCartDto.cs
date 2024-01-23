namespace WildBearAdventures.MVC.WildBear.Models.DTOs;
public class ShoppingCartDto
{
    public object billingAddress { get; set; }
    public Billingcurrency billingCurrency { get; set; }
    public object customer { get; set; }
    public object[] discounts { get; set; }
    public object[] shipments { get; set; }
    public DateTime createdDate { get; set; }
    public string cultureCode { get; set; }
    public object[] customProperties { get; set; }
    public decimal discount { get; set; }
    public decimal discountTotal { get; set; }
    public string id { get; set; }
    public object note { get; set; }
    public Orderline[] orderLines { get; set; }
    public decimal orderTotal { get; set; }
    public decimal paymentTotal { get; set; }
    public decimal shippingTotal { get; set; }
    public decimal subTotal { get; set; }
    public decimal tax { get; set; }
}

public class Billingcurrency
{
    public string id { get; set; }
    public string isoCode { get; set; }
}

public class Orderline
{
    public decimal discount { get; set; }
    public object[] discounts { get; set; }
    public string id { get; set; }
    public Orderlineproperty[] orderLineProperties { get; set; }
    public decimal price { get; set; }
    public string productName { get; set; }
    public int quantity { get; set; }
    public string sku { get; set; }
    public decimal tax { get; set; }
    public decimal taxRate { get; set; }
    public decimal total { get; set; }
    public decimal unitDiscount { get; set; }
    public string variantSku { get; set; }
}

public class Orderlineproperty
{
    public string id { get; set; }
    public string key { get; set; }
    public string value { get; set; }
}
