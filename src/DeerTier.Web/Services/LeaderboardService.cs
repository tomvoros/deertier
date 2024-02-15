using DeerTier.Web.Data;
using DeerTier.Web.Models;
using DeerTier.Web.Objects;

namespace DeerTier.Web.Services
{
    public class LeaderboardService
    {
        private readonly LeaderboardRepository _leaderboardRepository;
        private readonly ModerationService _moderationService;

        public LeaderboardService(LeaderboardRepository leaderboardRepository, ModerationService moderationService)
        {
            _leaderboardRepository = leaderboardRepository;
            _moderationService = moderationService;
        }

        public Record GetRecord(int id)
        {
            return _leaderboardRepository.GetRecord(id);
        }

        public Record[] GetRecords(int categoryId, bool excludeRecordsWithoutVideo = false)
        {
            return _leaderboardRepository.GetRecords(categoryId, excludeRecordsWithoutVideo);
        }

        public void AddRecord(UserContext context, Record record, bool isModeratorAction)
        {
            _leaderboardRepository.AddRecord(record);

            if (isModeratorAction)
            {
                _moderationService.LogSubmitRecord(context, record);
            }
        }

        public bool DeleteRecord(UserContext context, Record record)
        {
            bool result = _leaderboardRepository.DeleteRecord(record, context.IpAddress, context.User.Name);

            _moderationService.LogDeleteRecord(context, record);

            return result;
        }

        public Record[] GetAllRecords()
        {
            return _leaderboardRepository.GetAllRecords();
        }

        public ScoreDeletionRecordModel[] GetAllDeletedRecords()
        {
            return _leaderboardRepository.GetScoreDeletionLog();
        }
    }
}