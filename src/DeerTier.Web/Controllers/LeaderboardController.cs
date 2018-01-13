using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class LeaderboardController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LeaderboardController));

        private readonly LeaderboardService _leaderboardService;

        public LeaderboardController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
        }
        
        [HttpGet]
        public ActionResult Index(string categoryUrlName, int hideRecordsWithoutVideo = 0)
        {
            var category = CategoryService.GetCategoryByUrlName(categoryUrlName);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            var categoryModel = CategoryService.GetCategoryModel(category.Id);

            var viewModel = CreateViewModel<LeaderboardPageViewModel>();
            viewModel.Category = categoryModel;
            viewModel.HideRecordsWithoutVideo = hideRecordsWithoutVideo == 1;

            if (category.Parent != null)
            {
                viewModel.SectionHeading = category.Parent.Section.Name;
                viewModel.Heading = category.Parent.Name;

                viewModel.SiblingCategories = category.Parent.Subcategories
                    .Select(c => CategoryService.GetCategoryModel(c.Id)).ToArray(); ;
            }
            else
            {
                viewModel.SectionHeading = category.Section.Name;
                viewModel.Heading = category.Name;
            }
            
            IEnumerable<Record> records = _leaderboardService.GetRecords(category.Id, viewModel.HideRecordsWithoutVideo);
             
            if (category.GameTime && category.RealTime)
            {
                records = records.OrderBy(r => r.GameTimeSeconds).ThenBy(r => r.RealTimeSeconds).ThenBy(r => r.DateSubmitted);
            }
            else if (category.GameTime)
            {
                records = records.OrderBy(r => r.GameTimeSeconds).ThenBy(r => r.DateSubmitted);
            }
            else if (category.EscapeGameTime)
            {
                records = records.OrderByDescending(r => r.CeresTime).ThenBy(r => r.DateSubmitted);
            }
            else
            {
                records = records.OrderBy(r => r.RealTimeSeconds).ThenBy(r => r.DateSubmitted);
            }

            viewModel.Records = records
                .Select(r => MapRecord(r, category))
                .ToArray();

            for (int i = 0; i < viewModel.Records.Length; i++)
            {
                viewModel.Records[i].Rank = i + 1;
            }

            return View(viewModel);
        }

        private RecordModel MapRecord(Record record, Category category)
        {
            var recordModel = new RecordModel
            {
                Id = record.Id,
                Player = record.Player,
                RealTimeSeconds = record.RealTimeSeconds,
                RealTimeString = record.RealTimeString,
                GameTimeSeconds = record.GameTimeSeconds,
                GameTimeString = record.GameTimeString,
                Comment = record.Comment,
                VideoURL = record.VideoURL,
                CeresTime = record.CeresTime,
                DateSubmitted = record.DateSubmitted
            };

            if (category.RealTime)
            {
                var realTime = TimeSpan.FromSeconds(record.RealTimeSeconds);
                if (realTime.Hours == 0)
                {
                    recordModel.FormattedRealTime = string.Format("{0:mm\\:ss}", realTime);
                }
                else if (realTime.Hours < 10)
                {
                    recordModel.FormattedRealTime = string.Format("{0:h\\:mm\\:ss}", realTime);
                }
                else
                {
                    recordModel.FormattedRealTime = string.Format("{0:hh\\:mm\\:ss}", realTime);
                }
            }

            if (category.GameTime)
            {
                recordModel.FormattedGameTime = string.Format(@"{0:hh\:mm}", TimeSpan.FromSeconds(record.GameTimeSeconds));
            }

            if (category.EscapeGameTime)
            {
                recordModel.FormattedEscapeGameTime = record.CeresTime.ToString("##.00").Replace('.', '\'');
            }

            if (!string.IsNullOrWhiteSpace(record.Comment))
            {
                recordModel.HtmlComment = HttpUtility.HtmlEncode(record.Comment)
                    .Replace("FrankerZ", "<img src=\"/Images/FrankerZ.png\"/>");
            }

            return recordModel;
        }

        [HttpGet]
        [Authorize]
        public ActionResult SubmitTime(string categoryUrlName)
        {
            var category = CategoryService.GetCategoryByUrlName(categoryUrlName);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            return GetSubmitTimeView(category);
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult SubmitTime(string categoryUrlName, string gameTime, string escapeGameTime, string realTime, string videoLink, string comment, string username)
        {
            var category = CategoryService.GetCategoryByUrlName(categoryUrlName);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }

            if ((category.GameTime && string.IsNullOrWhiteSpace(gameTime))
                || (category.EscapeGameTime && string.IsNullOrWhiteSpace(escapeGameTime))
                || (category.RealTime && string.IsNullOrWhiteSpace(realTime)))
            {
                return GetSubmitTimeView(category, "Please fill out the time fields");
            }

            var isModeratorAction = false;

            if (IsModerator && !string.IsNullOrWhiteSpace(username))
            {
                // Allow moderators to submit for any username
                username = username.Trim();
                isModeratorAction = true;

                _logger.Info($"Moderator submitting record for another user: [{username}]");
            }
            else
            {
                username = Username;
            }

            var record = RecordUtil.CreateRecord(category, username, gameTime, escapeGameTime, realTime, videoLink, comment, UserId);
            if (record == null)
            {
                return GetSubmitTimeView(category, "Invalid time");
            }
            
            _leaderboardService.AddRecord(UserContext, record, isModeratorAction);

            _logger.Debug($"Record submitted: [{categoryUrlName}], [{gameTime}], [{escapeGameTime}], [{realTime}], [{videoLink}], [{comment}]");

            var viewModel = CreateViewModel<PageViewModel>();
            return View("SubmitSuccess", viewModel);
        }

        private ActionResult GetSubmitTimeView(Category category, string errorMessage = null)
        {
            var categoryModel = CategoryService.GetCategoryModel(category.Id);

            var viewModel = CreateViewModel<SubmitTimePageViewModel>();
            viewModel.Category = categoryModel;
            viewModel.ErrorMessage = errorMessage;

            return View(viewModel);
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult ModeratorDeleteRecord(int id)
        {
            if (!IsModerator)
            {
                _logger.Error($"Non-moderator attempted to delete record: [{id}]");
                return Content("unauthorized access");
            }
            
            _logger.Info($"Moderator deleting record: [{id}]");

            var record = _leaderboardService.GetRecord(id);

            if (record == null)
            {
                return new HttpNotFoundResult();
            }

            // Get category to ensure it's enabled
            var category = CategoryService.GetCategory(record.CategoryId);

            if (category == null)
            {
                return new HttpUnauthorizedResult();
            }
            
            if (!_leaderboardService.DeleteRecord(UserContext, record))
            {
                _logger.Error($"Failed to delete record: [{id}], [{record.Player}], [{category.UrlName}]");
                return Content("error deleting record");
            }
            
            return RedirectToAction("Index");
        }
    }
}