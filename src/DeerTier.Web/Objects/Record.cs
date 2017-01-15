using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeerTier.Web.Objects
{
    [Table("tblRecords")]
    public class Record
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public string Player { get; set; }
        public int RealTimeSeconds { get; set; }
        public string RealTimeString { get; set; }
        public int GameTimeSeconds { get; set; }
        public string GameTimeString { get; set; }
        public string Comment { get; set; }
        public string VideoURL { get; set; }
        public float CeresTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public int? SubmittedByUserId { get; set; }
    }
}