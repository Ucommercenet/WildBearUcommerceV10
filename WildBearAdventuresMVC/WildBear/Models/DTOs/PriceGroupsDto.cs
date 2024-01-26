namespace WildBearAdventures.MVC.WildBear.Models.DTOs
{

    public class PriceGroupsDto
    {
        public object nextPagingToken { get; set; }
        public object pagingToken { get; set; }
        public Pricegroup[] priceGroups { get; set; }
    }

    public class Pricegroup
    {
        public string id { get; set; }
        public string name { get; set; }
    }











}
