using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Misc.CustomRobotsTxt
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            var robotsTxtRoute = RouteTable.Routes["robots.txt"];
            routes.Remove(robotsTxtRoute);
            
            routes.MapRoute("robots.txt",
                            "robots.txt",
                            new { controller = "RobotsTxt", action = "RobotsTextFile" },
                            new[] { "Nop.Plugin.Misc.CustomRobotsTxt.Controllers" });
        }

        public int Priority
        {
            get
            {
                // The plugin's routes provider has to be executed after the standard
                // nopCommerce route providers. The reason is we have to overwrite robots.txt route.
                return -1;
            }
        }
    }
}
