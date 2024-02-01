namespace WildBearAdventures.MVC.WildBear.Models.DTOs
{
    public class CountryCollectionDto
    {
        public List<Country> Countries { get; set; }
        public object NextPagingToken { get; set; }
        public object PagingToken { get; set; }
    }

    public class Country
    {
        public string CultureCode { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

}
