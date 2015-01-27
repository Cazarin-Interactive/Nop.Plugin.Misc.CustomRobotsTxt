using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Plugin.Misc.CustomRobotsTxt.Models;
using Nop.Services.Blogs;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.News;
using Nop.Services.Seo;
using Nop.Services.Topics;
using Nop.Services.Vendors;

namespace Nop.Plugin.Misc.CustomRobotsTxt.Infrastructure
{
    class RobotsTxtManager
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IBlogService _blogService;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ITopicService _topicService;
        private readonly IVendorService _vendorService;
        private readonly IProductService _productService;
        private readonly int _storeId;
        private readonly IList<Language> _languages;

        public RobotsTxtManager(
            ISettingService settingService,
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
            _settingService = settingService;
            _localizationService = localizationService;
            _localizationSettings = localizationSettings;
            _storeContext = storeContext;
            _languageService = languageService;
            _blogService = blogService;
            _newsService = newsService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _topicService = topicService;
            _vendorService = vendorService;
            _productService = productService;
            _storeId = _storeContext.CurrentStore.Id;
            _languages = _languageService.GetAllLanguages(storeId: _storeId);
        }

        // Url helper to help rendering SEO friendly links. 
        // It must be set before GetRobotsTextFile method is executed.
        // The reason for having this property is UrlHelper is not initialized in
        // a controller constructor.
        public UrlHelper Url { get; set; }

        public RobotsTxtConfiguration GetConfiguration()
        {
            var config = new RobotsTxtConfiguration
            {
                All = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.All", false),
                BlogPost = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.BlogPost", false),
                Category = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.Category", false),
                Manufacturer = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.Manufacturer", false),
                NewsItem = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.NewsItem", false),
                Product = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.Product", false),
                Topic = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.Topic", false),
                Vendor = _settingService.GetSettingByKey("Plugins.Misc.CustomRobotsTxt.Vendor", false),
            };

            return config;
        }

        public void SaveConfiguration(RobotsTxtConfiguration config)
        {
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.All", config.All);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.BlogPost", config.BlogPost);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.Category", config.Category);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.Manufacturer", config.Manufacturer);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.NewsItem", config.NewsItem);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.Product", config.Product);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.Topic", config.Topic);
            _settingService.SetSetting("Plugins.Misc.CustomRobotsTxt.Vendor", config.Vendor);
        }

        public string GetRobotsTextFile()
        {
            var config = GetConfiguration();
            string content;
            if (config.All)
            {
                content = DisableAll();
            }
            else
            {
                content = DisableSelected(config);
            }

            return content;
        }

        private string DisableAll()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Disallow: /");
            return sb.ToString();
        }

        private string DisableSelected(RobotsTxtConfiguration config)
        {
            if (Url == null)
            {
                throw new InvalidOperationException("Url property cannot be null");
            }

            var sb = GetDefaultRobotsTextFile();
            if (config.BlogPost)
            {
                DisableBlogPosts(sb);
            }

            if (config.NewsItem)
            {
                DisableNews(sb);
            }

            if (config.Category)
            {
                DisableCategories(sb);
            }

            if (config.Manufacturer)
            {
                DisableManufacturers(sb);
            }

            if (config.Topic)
            {
                DisableTopics(sb);
            }

            if (config.Vendor)
            {
                DisableVendors(sb);
            }

            if (config.Product)
            {
                DisableProducts(sb);
            }

            return sb.ToString();
        }

        private void DisableProducts(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var products = _productService.SearchProducts();
            foreach(var product in products)
            {
                var seName = product.GetSeName();
                var url = Url.RouteUrl("Product", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);

                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                {
                    foreach (var language in _languages)
                    {
                        sb.AppendFormat("Disallow: {0}/productreviews/{1}", language.UniqueSeoCode, product.Id);
                        sb.Append(Environment.NewLine);
                    }
                }
                else
                {
                    sb.AppendFormat("Disallow: /productreviews/{0}", product.Id);
                    sb.Append(Environment.NewLine);
                }
            }

            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                foreach (var language in _languages)
                {
                    sb.AppendFormat("Disallow: {0}/producttag/all", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);

                    sb.AppendFormat("Disallow: {0}/search", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);

                    sb.AppendFormat("Disallow: {0}/recentlyviewedproducts", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);

                    sb.AppendFormat("Disallow: {0}/newproducts", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.AppendLine("Disallow: /producttag/all");
                sb.AppendLine("Disallow: /search");
                sb.AppendLine("Disallow: /recentlyviewedproducts");
                sb.AppendLine("Disallow: /newproducts");
            }
        }

        private void DisableVendors(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var vendors = _vendorService.GetAllVendors();
            foreach(var vendor in vendors)
            {
                var seName = vendor.GetSeName();
                var url = Url.RouteUrl("Vendor", new { SeName = seName });
                sb.AppendLine("Disallow: " + url); 
            }

            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                foreach (var language in _languages)
                {
                    sb.AppendFormat("Disallow: {0}/vendor/all/", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.AppendLine("Disallow: /vendor/all/");
            }
        }

        private void DisableTopics(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var topics = _topicService.GetAllTopics(_storeId);
            foreach (var topic in topics)
            {
                var seName = topic.GetSeName();
                var url = Url.RouteUrl("Topic", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);
            }
        }

        private void DisableManufacturers(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var manufacturers = _manufacturerService.GetAllManufacturers();
            foreach(var manufacturer in manufacturers)
            {
                var seName = manufacturer.GetSeName();
                var url = Url.RouteUrl("Manufacturer", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);
            }

            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                foreach (var language in _languages)
                {
                    sb.AppendFormat("Disallow: {0}/manufacturer/all/", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.AppendLine("Disallow: /manufacturer/all/");
            }
        }

        private void DisableCategories(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var categories = _categoryService.GetAllCategories();
            foreach (var category in categories)
            {
                var seName = category.GetSeName();
                var url = Url.RouteUrl("Category", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);
            }
        }

        private void DisableNews(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var news = _newsService.GetAllNews(0, _storeId, 0, int.MaxValue);
            foreach (var item in news)
            {
                var seName = item.GetSeName(item.LanguageId, ensureTwoPublishedLanguages: false);
                var url = Url.RouteUrl("NewsItem", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);
            }

            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                foreach (var language in _languages)
                {
                    sb.AppendFormat("Disallow: {0}/news", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.AppendLine("Disallow: /news");
            }
        }

        private void DisableBlogPosts(StringBuilder sb)
        {
            if (sb == null)
            {
                throw new ArgumentNullException("sb");
            }

            var posts = _blogService.GetAllBlogPosts(_storeId, 0, null, null, 0, int.MaxValue);
            foreach (var post in posts)
            {
                var seName = post.GetSeName(post.LanguageId, ensureTwoPublishedLanguages: false);
                var url = Url.RouteUrl("BlogPost", new { SeName = seName });
                sb.AppendLine("Disallow: " + url);
            }

            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                foreach (var language in _languages)
                {
                    sb.AppendFormat("Disallow: {0}/blog", language.UniqueSeoCode);
                    sb.Append(Environment.NewLine);
                }
            }
            else
            {
                sb.AppendLine("Disallow: /blog");
            }
        }

        private StringBuilder GetDefaultRobotsTextFile()
        {
            // This code had been taken from \Presentation\Nop.Web\Controllers\CommonController.cs.
            // Make sure it is synchronized in the future versions.

            var disallowPaths = new List<string>
                                    {
                                        "/bin/",
                                        "/content/files/",
                                        "/content/files/exportimport/",
                                        "/country/getstatesbycountryid",
                                        "/install",
                                        "/setproductreviewhelpfulness",
                                    };
            var localizableDisallowPaths = new List<string>
                                               {
                                                   "/addproducttocart/catalog/",
                                                   "/addproducttocart/details/",
                                                   "/backinstocksubscriptions/manage",
                                                   "/boards/forumsubscriptions",
                                                   "/boards/forumwatch",
                                                   "/boards/postedit",
                                                   "/boards/postdelete",
                                                   "/boards/postcreate",
                                                   "/boards/topicedit",
                                                   "/boards/topicdelete",
                                                   "/boards/topiccreate",
                                                   "/boards/topicmove",
                                                   "/boards/topicwatch",
                                                   "/cart",
                                                   "/checkout",
                                                   "/checkout/billingaddress",
                                                   "/checkout/completed",
                                                   "/checkout/confirm",
                                                   "/checkout/shippingaddress",
                                                   "/checkout/shippingmethod",
                                                   "/checkout/paymentinfo",
                                                   "/checkout/paymentmethod",
                                                   "/clearcomparelist",
                                                   "/compareproducts",
                                                   "/customer/avatar",
                                                   "/customer/activation",
                                                   "/customer/addresses",
                                                   "/customer/changepassword",
                                                   "/customer/checkusernameavailability",
                                                   "/customer/downloadableproducts",
                                                   "/customer/info",
                                                   "/deletepm",
                                                   "/emailwishlist",
                                                   "/inboxupdate",
                                                   "/newsletter/subscriptionactivation",
                                                   "/onepagecheckout",
                                                   "/order/history",
                                                   "/orderdetails",
                                                   "/passwordrecovery/confirm",
                                                   "/poll/vote",
                                                   "/privatemessages",
                                                   "/returnrequest",
                                                   "/returnrequest/history",
                                                   "/rewardpoints/history",
                                                   "/sendpm",
                                                   "/sentupdate",
                                                   "/shoppingcart/productdetails_attributechange",
                                                   "/subscribenewsletter",
                                                   "/topic/authenticate",
                                                   "/viewpm",
                                                   "/uploadfileproductattribute",
                                                   "/uploadfilecheckoutattribute",
                                                   "/wishlist",
                                               };


            var sb = new StringBuilder();
            sb.Append("User-agent: *");
            sb.Append(Environment.NewLine);

            //usual paths
            foreach (var path in disallowPaths)
            {
                sb.AppendFormat("Disallow: {0}", path);
                sb.Append(Environment.NewLine);
            }
            //localizable paths (without SEO code)
            foreach (var path in localizableDisallowPaths)
            {
                sb.AppendFormat("Disallow: {0}", path);
                sb.Append(Environment.NewLine);
            }
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //URLs are localizable. Append SEO code
                foreach (var language in _languages)
                {
                    foreach (var path in localizableDisallowPaths)
                    {
                        sb.AppendFormat("Disallow: {0}{1}", language.UniqueSeoCode, path);
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            return sb;
        }
    }
}
