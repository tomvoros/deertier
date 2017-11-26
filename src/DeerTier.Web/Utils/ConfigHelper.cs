using System.Configuration;

namespace DeerTier.Web.Utils
{
    public static class ConfigHelper
    {
        public static readonly string AdminKey = ConfigurationManager.AppSettings["AdminKey"];

        public static readonly string ApiKey = ConfigurationManager.AppSettings["ApiKey"];

        public static readonly string DiscordUrl = ConfigurationManager.AppSettings["DiscordUrl"];
    }
}