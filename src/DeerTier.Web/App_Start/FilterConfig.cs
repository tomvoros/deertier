using DeerTier.Web.Filters;
using System.Web.Mvc;

namespace DeerTier.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Filters will execute in the order listed
            filters.Add(new HandleErrorAttribute(), 1);
            filters.Add(new ErrorLoggingFilter(), 0);
        }
    }
}