namespace WildBearAdventures.MVC.WildBear.Models.DTOs
{
    public class CountriesDto
    {
        public Country[] countries { get; set; }
        public object nextPagingToken { get; set; }
        public object pagingToken { get; set; }
    }

    public class Country
    {
        public string cultureCode { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }

}
