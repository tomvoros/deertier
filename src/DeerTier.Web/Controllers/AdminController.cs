using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly LeaderboardService _leaderboardService;
        private readonly WebContentService _webContentService;

        public AdminController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService, WebContentService webContentService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
            _webContentService = webContentService;
        }

        [HttpGet]
        public ActionResult Moderators(string adminKey)
        {
            if (adminKey != ConfigHelper.AdminKey)
            {
                return Content("unauthorized access");
            }
            
            var usernames = AccountService.GetUsernames();
            
            return View(usernames);
        }

        // TODO: secure this
        //[HttpPost]
        //public ActionResult Moderators(string modOrDemod, string username)
        //{
        //    var content = "no action taken";
            
        //    if (modOrDemod == "mod")
        //    {
        //        content = (AccountService.ModUser(username) ? (username + " successfully modded") : ("error modding " + username));
        //    }
        //    else if (modOrDemod == "de-mod")
        //    {
        //        content = (AccountService.DeModUser(username) ? (username + " successfully de-modded") : ("error de-modding " + username));
        //    }
            
        //    return Content(content);
        //}

        [HttpGet]
        public ActionResult ScoreDeletionLog(string adminKey)
        {
            if (adminKey != ConfigHelper.AdminKey)
            {
                return Content("unauthorized access");
            }
            
            var deletedRecords = _leaderboardService.GetAllDeletedRecords();
            
            return View(deletedRecords);
        }

        [HttpGet]
        public ActionResult FlushWebContentCache()
        {
            _webContentService.FlushCache();

            return Content("ok");
        }
    }
}