namespace DeerTier.Web.Models
{
    public class HomePageViewModel : PageViewModel
    {
        public string EmbeddedHtmlContent { get; set; }
        public string FormattedModerators { get; set; }
        public string DiscordUrl { get; set; }
    }
}