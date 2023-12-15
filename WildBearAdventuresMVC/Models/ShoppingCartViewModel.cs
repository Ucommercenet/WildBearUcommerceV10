namespace WildBearAdventuresMVC.Models
{
    public class ShoppingCartViewModel
    {
        public required List<OrderLineViewModel> ShoppingChartOrderLineViewModels { get; set; }

        public decimal ShoppingCartOrderTotal { get; set; }


        //public decimal ShoppingCartPaymentTotal { get; set; }
        //public decimal ShoppingCartShippingTotal { get; set; }

    }





}
