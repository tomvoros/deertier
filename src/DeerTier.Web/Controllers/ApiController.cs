using DeerTier.Web.Filters;
using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using DeerTier.Web.Utils;
using log4net;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DeerTier.Web.Controllers
{
    [ApiError]
    public class ApiController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ApiController));

        private readonly LeaderboardService _leaderboardService;
        
        public ApiController(AccountService accountService, CategoryService categoryService, LeaderboardService leaderboardService)
            : base(accountService, categoryService)
        {
            _leaderboardService = leaderboardService;
        }
        
        [HttpGet]
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
        
        [HttpPost]
        [ApiAuthorize]
        public ActionResult SubmitRecord(string username, string category, string realTime, string gameTime, string escapeGameTime, string videoUrl, string comment)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Username required");
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Category required");
            }

            var categoryObj = CategoryService.GetCategoryByUrlName(category);
            if (categoryObj == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid category");
            }

            if ((categoryObj.GameTime && string.IsNullOrWhiteSpace(gameTime))
                || (categoryObj.EscapeGameTime && string.IsNullOrWhiteSpace(escapeGameTime))
                || (categoryObj.RealTime && string.IsNullOrWhiteSpace(realTime)))
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Time required");
            }

            var record = RecordUtil.CreateRecord(categoryObj, username, gameTime, escapeGameTime, realTime, videoUrl, comment, UserId);
            if (record == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Invalid time");
            }
            
            _leaderboardService.AddRecord(UserContext, record, true);
            
            _logger.Debug($"Record submitted: [{category}], [{gameTime}], [{escapeGameTime}], [{realTime}], [{videoUrl}], [{comment}]");

            var mappedRecord = MapRecord(record);
            return Json(mappedRecord);
        }

        [HttpDelete]
        [ApiAuthorize]
        public ActionResult DeleteRecord(int id)
        {
            _logger.Info($"Moderator deleting record: [{id}]");

            var record = _leaderboardService.GetRecord(id);
            if (record == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Record not found");
            }

            // Get category to ensure it's enabled
            var category = CategoryService.GetCategory(record.CategoryId);
            if (category == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Can't delete records in a disabled category");
            }

            if (!_leaderboardService.DeleteRecord(UserContext, record))
            {
                _logger.Error($"Failed to delete record: [{id}], [{record.Player}], [{category.UrlName}]");
                throw new ApiException("Failed to delete record");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        
        protected override User GetUser()
        {
            return AccountService.GetUser("Api");
        }
    }
}