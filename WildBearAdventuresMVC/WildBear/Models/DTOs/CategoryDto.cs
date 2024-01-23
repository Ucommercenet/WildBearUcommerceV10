namespace WildBearAdventures.MVC.WildBear.Models.DTOs
{
    public class CategoryDto
    {
        public string CatalogId { get; set; }
        public object[] CategoryIds { get; set; }
        public string DefinitionId { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public object ImageMediaId { get; set; }
        public object ImageMediaUrl { get; set; }
        public object ParentCategoryId { get; set; }
        public string[] ProductIds { get; set; }
        public int SortOrder { get; set; }
        public DateTime __lastIndexed { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }


    }
}
