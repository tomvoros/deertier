using System;

namespace DeerTier.Web.Models
{
    public class ScoreDeletionRecordModel
    {
        public int ID { get; set; }
        public string Moderator { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int CategoryId { get; set; }
        public string Player { get; set; }
        public string RealTimeString { get; set; }
        public string GameTimeString { get; set; }
        public int RealTimeSeconds { get; set; }
        public int GameTimeSeconds { get; set; }
        public string Comment { get; set; }
        public string VideoURL { get; set; }
        public double CeresTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public int? SubmittedByUserId { get; set; }
        public string IPAddress { get; set; }
    }
}