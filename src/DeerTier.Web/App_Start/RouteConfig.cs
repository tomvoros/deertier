using System.Web.Mvc;
using System.Web.Routing;

namespace DeerTier.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Leaderboard_SubmitTime",
                url: "Leaderboard/{categoryUrlName}/Submit",
                defaults: new { controller = "Leaderboard", action = "SubmitTime" }
            );

            routes.MapRoute(
                name: "Leaderboard_ModeratorDeleteRecord",
                url: "Leaderboard/{categoryUrlName}/ModeratorDeleteRecord",
                defaults: new { controller = "Leaderboard", action = "ModeratorDeleteRecord" }
            );

            routes.MapRoute(
                name: "Leaderboard",
                url: "Leaderboard/{categoryUrlName}",
                defaults: new { controller = "Leaderboard", action = "Index" }
            );

            routes.MapRoute(
                name: "News",
                url: "News",
                defaults: new { controller = "Home", action = "News" }
            );

            routes.MapRoute(
                name: "Api/GetAllRecords",
                url: "api/records",
                defaults: new { controller = "Api", action = "GetAllRecords" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") }
            );

            routes.MapRoute(
                name: "Api/SubmitRecord",
                url: "api/records",
                defaults: new { controller = "Api", action = "SubmitRecord" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
            );

            routes.MapRoute(
                name: "Api/DeleteRecord",
                url: "api/records/{id}",
                defaults: new { controller = "Api", action = "DeleteRecord" },
                constraints: new { httpMethod = new HttpMethodConstraint("DELETE") }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
