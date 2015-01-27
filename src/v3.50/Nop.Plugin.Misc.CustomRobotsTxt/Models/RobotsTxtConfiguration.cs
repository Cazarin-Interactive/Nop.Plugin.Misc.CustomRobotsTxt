using Nop.Web.Framework;

namespace Nop.Plugin.Misc.CustomRobotsTxt.Models
{
    public class RobotsTxtConfiguration
    {
        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.BlogPost")]
        public bool BlogPost { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.Category")]
        public bool Category { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.Manufacturer")]
        public bool Manufacturer { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.NewsItem")]
        public bool NewsItem { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.Product")]
        public bool Product { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.Topic")]
        public bool Topic { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.Vendor")]
        public bool Vendor { get; set; }

        [NopResourceDisplayName("Plugins.Misc.CustomRobotsTxt.All")]
        public bool All { get; set; }
    }
}