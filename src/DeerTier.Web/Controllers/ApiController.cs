using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class ApiController : BaseController
    {
        private readonly LeaderboardService _leaderboardService;

        public ApiController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
        }
        
        public ActionResult GetAllRecords()
        {
            var records = _leaderboardService.GetAllRecords();

            var formattedRecords = records
                .Select(MapRecord)
                .Where(r => r != null);

            return Json(formattedRecords, JsonRequestBehavior.AllowGet);
        }

        private object MapRecord(Record record)
        {
            var category = CategoryService.GetCategory(record.CategoryId);

            if (category == null)
            {
                return null;
            }

            string formattedRealTime = null;
            string formattedGameTime = null;
            string formattedEscapeGameTime = null;

            if (category.RealTime)
            {
                var realTime = TimeSpan.FromSeconds(record.RealTimeSeconds);
                if (realTime.Hours == 0)
                {
                    formattedRealTime = string.Format("{0:mm\\:ss}", realTime);
                }
                else if (realTime.Hours < 10)
                {
                    formattedRealTime = string.Format("{0:h\\:mm\\:ss}", realTime);
                }
                else
                {
                    formattedRealTime = string.Format("{0:hh\\:mm\\:ss}", realTime);
                }
            }

            if (category.GameTime)
            {
                formattedGameTime = string.Format(@"{0:hh\:mm}", TimeSpan.FromSeconds(record.GameTimeSeconds));
            }

            if (category.EscapeGameTime)
            {
                formattedEscapeGameTime = record.CeresTime.ToString("##.00").Replace('.', '\'');
            }

            return new
            {
                ID = record.Id,
                Username = record.Player,
                Category = category.UrlName,
                RealTime = formattedRealTime,
                GameTime = formattedGameTime,
                EscapeGameTime = formattedEscapeGameTime,
                VideoUrl = record.VideoURL,
                Comment = record.Comment,
                DateSubmitted = record.DateSubmitted
            };
        }
    }
}