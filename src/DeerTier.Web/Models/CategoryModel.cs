namespace DeerTier.Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string UrlName { get; set; }
        public string LinkUrl { get; set; }
        public bool GameTime { get; set; }
        public bool EscapeGameTime { get; set; }
        public bool RealTime { get; set; }
        public string SectionName { get; set; }
        public string WikiUrl { get; set; }
    }
}