namespace WildBearAdventuresMVC.WildBear.Models.DTOs;
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
    public float discount { get; set; }
    public float discountTotal { get; set; }
    public string id { get; set; }
    public object note { get; set; }
    public Orderline[] orderLines { get; set; }
    public float orderTotal { get; set; }
    public float paymentTotal { get; set; }
    public float shippingTotal { get; set; }
    public float subTotal { get; set; }
    public float tax { get; set; }
}

public class Billingcurrency
{
    public string id { get; set; }
    public string isoCode { get; set; }
}

public class Orderline
{
    public float discount { get; set; }
    public object[] discounts { get; set; }
    public string id { get; set; }
    public Orderlineproperty[] orderLineProperties { get; set; }
    public float price { get; set; }
    public string productName { get; set; }
    public int quantity { get; set; }
    public string sku { get; set; }
    public float tax { get; set; }
    public float taxRate { get; set; }
    public float total { get; set; }
    public float unitDiscount { get; set; }
    public string variantSku { get; set; }
}

public class Orderlineproperty
{
    public string id { get; set; }
    public string key { get; set; }
    public string value { get; set; }
}
