using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using SpeedrunComSharp;
using System;
using System.Linq;
using System.Web.Mvc;
using DtRecord = DeerTier.Web.Objects.Record;
using SrcRecord = SpeedrunComSharp.Record;

namespace DeerTier.Web.Controllers
{
    public class SpeedrunComLeaderboardController : LeaderboardBaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SpeedrunComLeaderboardController));

        private readonly LeaderboardService _leaderboardService;

        public SpeedrunComLeaderboardController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public ActionResult Index(string categoryUrlName)
        {
            var category = CategoryService.GetCategoryByUrlName(categoryUrlName);

            if (category == null || string.IsNullOrWhiteSpace(category.SpeedrunComCategoryId))
            {
                return new HttpNotFoundResult();
            }
            
            var srcClient = new SpeedrunComClient();
            var srcLeaderboard = srcClient.Leaderboards.GetLeaderboardForFullGameCategory(ConfigHelper.SpeedRunComGameId, category.SpeedrunComCategoryId);

            var records = srcLeaderboard.Records.Select(MapRecord);
            var viewModel = CreateLeaderboardViewModel(category, records);

            return View("../Leaderboard/Index", viewModel);
        }

        private DtRecord MapRecord(SrcRecord srcRecord)
        {
            var record = new DtRecord
            {
                Id = 0,
                Player = srcRecord.Player.Name,
                RealTimeSeconds = (int)Math.Round(srcRecord.Times.RealTime?.TotalSeconds ?? 0),
                GameTimeSeconds = (int)Math.Round(srcRecord.Times.GameTime?.TotalSeconds ?? 0),
                Comment = srcRecord.Comment,
                VideoURL = srcRecord.Videos?.Links?.FirstOrDefault()?.AbsoluteUri,
                CeresTime = 0,
                DateSubmitted = srcRecord.Date ?? srcRecord.DateSubmitted
            };

            return record;
        }
    }
}