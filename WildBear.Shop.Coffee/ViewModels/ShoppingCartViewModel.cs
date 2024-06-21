namespace WildBearAdventures.MVC.ViewModels
{
    public class ShoppingCartViewModel
    {
        public required List<OrderLineViewModel> ShoppingChartOrderLineViewModels { get; set; }

        public decimal ShoppingCartOrderTotal { get; set; }

        public Guid ShoppingCartGuid { get; set; }


    }





}
