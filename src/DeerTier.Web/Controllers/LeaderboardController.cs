using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    public class LeaderboardController : LeaderboardBaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LeaderboardController));

        private readonly LeaderboardService _leaderboardService;

        public LeaderboardController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
        }
        
        [HttpGet]
        public ActionResult Index(string categoryUrlName)
        {
            var category = CategoryService.GetCategoryByUrlName(categoryUrlName);

            if (category == null)
            {
                return new HttpNotFoundResult();
            }
            
            IEnumerable<Record> records = _leaderboardService.GetRecords(category.Id);
             
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

            var viewModel = CreateLeaderboardViewModel(category, records);
            
            return View(viewModel);
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

            var record = CreateRecord(category, username, gameTime, escapeGameTime, realTime, videoLink, comment);
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
        
        private Record CreateRecord(Category category, string player, string gameTime, string escapeGameTime, string realTime, string videoLink, string comment)
        {
            var record = new Record
            {
                CategoryId = category.Id,
                Player = player,
                VideoURL = videoLink,
                Comment = comment,
                DateSubmitted = DateTime.Now,
                SubmittedByUserId = UserId
            };
            
            if (category.GameTime)
            {
                var formattedTime = TimeUtil.GetFormattedTime(gameTime);
                if (formattedTime.TimeSeconds == -1)
                {
                    return null;
                }

                record.GameTimeSeconds = formattedTime.TimeSeconds * 60;
                record.GameTimeString = formattedTime.TimeString;
            }

            if (category.RealTime)
            {
                var formattedTime = TimeUtil.GetFormattedTime(realTime);
                if (formattedTime.TimeSeconds == -1)
                {
                    return null;
                }

                record.RealTimeSeconds = formattedTime.TimeSeconds;
                record.RealTimeString = formattedTime.TimeString;
            }

            if (category.EscapeGameTime)
            {
                if (escapeGameTime.IndexOf('\'') < 0)
                {
                    return null;
                }

                if (escapeGameTime.Split(new [] { '\'' })[1].Length != 2)
                {
                    return null;
                }

                try
                {
                    record.CeresTime = float.Parse(escapeGameTime.Replace("'", "."));
                }
                catch (Exception)
                {
                    return null;
                }
            }

            // Normalize miissing/empty time strings (for legacy purposes)
            record.GameTimeString = record.GameTimeString ?? "";
            record.RealTimeString = record.RealTimeString ?? "";

            return record;
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