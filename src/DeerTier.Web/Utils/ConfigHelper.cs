using System.Configuration;

namespace DeerTier.Web.Utils
{
    public static class ConfigHelper
    {
        public static readonly string AdminKey = ConfigurationManager.AppSettings["AdminKey"];
    }
}