namespace WildBearAdventures.MVC.WildBear.Models.DTOs
{

    public class PriceGroupsDto
    {
        public object nextPagingToken { get; set; }
        public object pagingToken { get; set; }
        public List<PriceGroup> PriceGroups { get; set; }
    }

    public class PriceGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }











}
