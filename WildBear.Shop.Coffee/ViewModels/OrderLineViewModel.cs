namespace WildBear.Shop.Coffee.ViewModels
{
    public class OrderLineViewModel
    {
        public string sku { get; set; }
        public string variantSku { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }


        //Price Related
        public decimal total { get; set; }
        public decimal price { get; set; }
        public decimal tax { get; set; }
        public decimal taxRate { get; set; }


    }
}
