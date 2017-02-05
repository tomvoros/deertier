using DeerTier.Web.Models;
using DeerTier.Web.Objects;
using DeerTier.Web.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerTier.Web.Controllers
{
    public abstract class LeaderboardBaseController : BaseController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LeaderboardBaseController));

        public LeaderboardBaseController(AccountService accountService, CategoryService categoryService)
            : base(accountService, categoryService) { }

        protected LeaderboardPageViewModel CreateLeaderboardViewModel(Category category, IEnumerable<Record> records)
        {
            var categoryModel = CategoryService.GetCategoryModel(category.Id);

            var viewModel = CreateViewModel<LeaderboardPageViewModel>();
            viewModel.Category = categoryModel;

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
            
            viewModel.Records = records
                .Select(r => MapRecord(r, category))
                .ToArray();

            for (int i = 0; i < viewModel.Records.Length; i++)
            {
                viewModel.Records[i].Rank = i + 1;
            }

            return viewModel;
        }

        protected RecordModel MapRecord(Record record, Category category)
        {
            var recordModel = new RecordModel
            {
                Id = record.Id,
                Player = record.Player,
                RealTimeSeconds = record.RealTimeSeconds,
                GameTimeSeconds = record.GameTimeSeconds,
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
    }
}