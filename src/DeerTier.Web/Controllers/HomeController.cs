using DeerTier.Web.Models;
using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using System.Linq;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(AccountService accountService, CategoryService categoryService)
            : base(accountService, categoryService) { }

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
            viewModel.FormattedModerators = text;
            viewModel.DiscordUrl = ConfigHelper.DiscordUrl;

            return View(viewModel);
        }

        public ActionResult News()
        {
            var viewModel = CreateViewModel<PageViewModel>();
            return View(viewModel);
        }
    }
}