using DeerTier.Web.Objects;
using System;

namespace DeerTier.Web.Utils
{
    public class RecordUtil
    {
        public static Record CreateRecord(Category category, string player, string gameTime, string escapeGameTime, string realTime, string videoLink, string comment, int submittedByUserId)
        {
            var record = new Record
            {
                CategoryId = category.Id,
                Player = player,
                VideoURL = videoLink ?? "",
                Comment = comment ?? "",
                DateSubmitted = DateTime.Now,
                SubmittedByUserId = submittedByUserId
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

                if (escapeGameTime.Split(new[] { '\'' })[1].Length != 2)
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

            // Normalize missing/empty time strings (for legacy purposes)
            record.GameTimeString = record.GameTimeString ?? "";
            record.RealTimeString = record.RealTimeString ?? "";

            return record;
        }
    }
}