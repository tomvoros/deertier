using DeerTier.Web.Models;
using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        private readonly WebContentService _webContentService;

        public HomeController(AccountService accountService, CategoryService categoryService, WebContentService webContentService)
            : base(accountService, categoryService)
        {
            _webContentService = webContentService;
        }

        public ActionResult Index()
        {
            var moderators = AccountService.GetModerators()
                .OrderBy(m => m)
                .ToList();
           
            string text = "";
            for (int i = 0; i < moderators.Count; i++)
            {
                if (i == moderators.Count - 2)
                {
                    text = text + moderators[i] + " and ";
                }
                else if (i == moderators.Count - 1)
                {
                    text += moderators[i];
                }
                else
                {
                    text = text + moderators[i] + ", ";
                }
            }

            var viewModel = CreateViewModel<HomePageViewModel>();
            viewModel.EmbeddedHtmlContent = GetHomepageContent();
            viewModel.FormattedModerators = text;
            viewModel.DiscordUrl = ConfigHelper.DiscordUrl;

            return View(viewModel);
        }

        private string GetHomepageContent()
        {
            try
            {
                var homepageContent = _webContentService.GetContent(ConfigHelper.HomepageContentUrl);
                if (string.IsNullOrWhiteSpace(homepageContent))
                {
                    throw new Exception("No homepage content");
                }
                return homepageContent;
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get homepage content", ex);
                return "[Failed to load homepage content.]";
            }
        }

        public ActionResult News()
        {
            var viewModel = CreateViewModel<PageViewModel>();
            return View(viewModel);
        }
    }
}