using DeerTier.Web.Data;
using DeerTier.Web.Objects;
using System;

namespace DeerTier.Web.Services
{
    public class ModerationService
    {
        private readonly ModerationRepository _moderationRepository;

        public ModerationService(ModerationRepository moderationRepository)
        {
            _moderationRepository = moderationRepository;
        }

        public void LogSubmitRecord(UserContext context, Record record)
        {
            var action = CreateModerationAction(context, ModerationActionType.SubmitRecord);
            action.Description = $"Created record [{record.Id}] for user [{record.Player}] in category [{record.CategoryId}]";
            action.RelatedId1 = record.Id;
            LogAction(action);
        }

        public void LogDeleteRecord(UserContext context, Record record)
        {
            var action = CreateModerationAction(context, ModerationActionType.DeleteRecord);
            action.Description = $"Deleted record [{record.Id}] for user [{record.Player}] in category [{record.CategoryId}]";
            action.RelatedId1 = record.Id;
            LogAction(action);
        }

        private ModerationAction CreateModerationAction(UserContext context, ModerationActionType action)
        {
            return new ModerationAction
            {
                UserId = context.User.Id,
                Action = action,
                Date = DateTime.Now,
                IpAddress = context.IpAddress,
                UserAgent = context.UserAgent
            };
        }

        private void LogAction(ModerationAction action)
        {
            _moderationRepository.LogModerationAction(action);
        }
    }
}