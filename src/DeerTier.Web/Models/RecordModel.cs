using System;
using System.Web;

namespace DeerTier.Web.Models
{
    public class RecordModel
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public int RealTimeSeconds { get; set; }
        public int GameTimeSeconds { get; set; }
        public string Comment { get; set; }
        public string VideoURL { get; set; }
        public float CeresTime { get; set; }
        public string FormattedRealTime { get; set; }
        public string FormattedGameTime { get; set; }
        public string FormattedEscapeGameTime { get; set; }
        public string HtmlComment { get; set; }
        public DateTime? DateSubmitted { get; set; }

        public int Rank { get; set; }

        public string VideoURLAsLink
        {
            get
            {
                if (string.IsNullOrWhiteSpace(VideoURL))
                {
                    return "";
                }

                // Ensure the URL is not relative
                var url = VideoURL.Trim();
                if (!url.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) &&
                    !url.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) &&
                    !url.StartsWith("//"))
                {
                    url = "http://" + url;
                }

                var icon = "fa-video-camera";

                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    var host = uri.Host.ToLowerInvariant();
                    if (host.EndsWith("twitch.tv"))
                    {
                        icon = "fa-twitch";
                    }
                    else if (host.EndsWith("youtube.com") || host.EndsWith("youtu.be"))
                    {
                        icon = "fa-youtube-play";
                    }
                }

                return $"<a href=\"{HttpUtility.HtmlAttributeEncode(url)}\" target=\"_blank\"><i class=\"fa {icon}\" aria-hidden=\"true\"></i></a>";
            }
        }

        public string DateSubmittedAsString
        {
            get
            {
                if (DateSubmitted.HasValue)
                {
                    return DateSubmitted.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }

        public long DateSubmittedSortOrder
        {
            get
            {
                return DateSubmitted.HasValue ? DateSubmitted.Value.Ticks : 0;
            }
        }

        public string RankClass
        {
            get
            {
                switch (Rank)
                {
                    case 1:
                        return "gold";
                    case 2:
                        return "silver";
                    case 3:
                        return "bronze";
                    default:
                        return null;
                }
            }
        }
    }
}