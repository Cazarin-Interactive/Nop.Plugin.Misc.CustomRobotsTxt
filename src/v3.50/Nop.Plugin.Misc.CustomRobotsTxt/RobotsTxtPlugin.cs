using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.CustomRobotsTxt
{
    public class RobotsTxtPlugin : BasePlugin, IMiscPlugin
    {
        private readonly ISettingService _settingService;

        public RobotsTxtPlugin(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "RobotsTxt";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.Misc.CustomRobotsTxt.Controllers" }, { "area", null } };
        }

        public override void Install()
        {
            InstallLocalization();
            base.Install();
        }

        private void InstallLocalization()
        {
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Description", "Choose what nopCommerce objects you are going to hide from crawlers. You can upload <b>__robots.txt</b> file the web-site root to override plugin settings.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.BlogPost", "Blog posts");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Category", "Categories");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Manufacturer", "Manufacturers");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.NewsItem", "News");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Product", "Products");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Topic", "Topics");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Vendor", "Vendors");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.All", "All");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.BlogPost.Hint", "Check it to prevent crawling blog posts.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Category.Hint", "Check it to prevent crawling categories.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Manufacturer.Hint", "Check it to prevent crawling manufacturers.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.NewsItem.Hint", "Check it to prevent crawling news.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Product.Hint", "Check it to prevent crawling products.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Topic.Hint", "Check it to prevent crawling topics.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Vendor.Hint", "Check it to prevent crawling vendors.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.All.Hint", "Check it to prevent crawling all pages on the website.");
        }

        public override void Uninstall()
        {
            RemoveLocalization();
            RemoveSettings();
            base.Uninstall();
        }

        private void RemoveSettings()
        {
            var setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.BlogPost");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.Category");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.Manufacturer");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.NewsItem");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.Product");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.Topic");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.All");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }

            setting = _settingService.GetSetting("Plugins.Misc.CustomRobotsTxt.Vendor");
            if (setting != null)
            {
                _settingService.DeleteSetting(setting);
            }
        }

        private void RemoveLocalization()
        {
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Description");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.BlogPost");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Category");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Manufacturer");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.NewsItem");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Product");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Topic");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Vendor");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.All");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.BlogPost.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Category.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Manufacturer.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.NewsItem.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Product.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Topic.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.Vendor.Hint");
            this.DeletePluginLocaleResource("Plugins.Misc.CustomRobotsTxt.All.Hint");
        }
    }
}
