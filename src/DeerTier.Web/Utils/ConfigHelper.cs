using System.Configuration;

namespace DeerTier.Web.Utils
{
    public static class ConfigHelper
    {
        public static readonly string AdminKey = ConfigurationManager.AppSettings["AdminKey"];

        public static readonly string DiscordUrl = ConfigurationManager.AppSettings["DiscordUrl"];

        public static readonly string SpeedRunComGameId = ConfigurationManager.AppSettings["SpeedrunCom.GameId"];
    }
}