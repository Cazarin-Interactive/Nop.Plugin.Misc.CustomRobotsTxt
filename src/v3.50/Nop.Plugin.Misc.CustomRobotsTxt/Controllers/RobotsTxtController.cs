using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Plugin.Misc.CustomRobotsTxt.Infrastructure;
using Nop.Plugin.Misc.CustomRobotsTxt.Models;
using Nop.Services.Blogs;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.News;
using Nop.Services.Topics;
using Nop.Services.Vendors;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Misc.CustomRobotsTxt.Controllers
{
    public class RobotsTxtController : BasePluginController
    {
        private const string ConfigureView = "~/Plugins/Misc.CustomRobotsTxt/Views/RobotsTxt/Configure.cshtml";

        private readonly RobotsTxtManager _manager;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        public RobotsTxtController(ISettingService settingService,
            ILocalizationService localizationService,
            LocalizationSettings localizationSettings,
            IStoreContext storeContext,
            ILanguageService languageService,
            IBlogService blogService,
            INewsService newsService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            ITopicService topicService,
            IVendorService vendorService,
            IProductService productService)
        {
            _manager = new RobotsTxtManager(settingService, localizationService, localizationSettings,
                storeContext, languageService, blogService, newsService, categoryService, manufacturerService,
                topicService, vendorService, productService);
            _localizationService = localizationService;
            _settingService = settingService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = _manager.GetConfiguration();
            return View(ConfigureView, model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(RobotsTxtConfiguration model)
        {
            _manager.SaveConfiguration(model);
            _settingService.ClearCache();
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        public ActionResult RobotsTextFile()
        {
            var robotsTextFile = Server.MapPath("~/__robots.txt");
            if (System.IO.File.Exists(robotsTextFile))
            {
                return File(robotsTextFile, "text/plain");
            }

            _manager.Url = Url;
            var content = _manager.GetRobotsTextFile();
            Response.ContentType = "text/plain";
            Response.Write(content);
            return null;
        }
    }
}
