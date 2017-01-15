namespace DeerTier.Web.Models
{
    public class LeaderboardPageViewModel : PageViewModel
    {
        public string SectionHeading { get; set; }
        public string Heading { get; set; }
        public CategoryModel[] SiblingCategories { get; set; }
        public CategoryModel Category { get; set; }
        public RecordModel[] Records { get; set; }
    }
}