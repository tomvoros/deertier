namespace DeerTier.Web.Models
{
    public class PageViewModel
    {
        public bool IsAuthenticated { get; set; }
        public bool IsModerator { get; set; }
        public string Username { get; set; }
        public MainCategoriesModel MainCategories { get; set; }
    }
}