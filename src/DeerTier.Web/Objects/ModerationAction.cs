using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeerTier.Web.Objects
{
    [Table("tblModerationLog")]
    public class ModerationAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public ModerationActionType Action { get; set; }
        public string Description { get; set; }
        public int? RelatedId1 { get; set; }
        public int? RelatedId2 { get; set; }
        public int? RelatedId3 { get; set; }
        public DateTime Date { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}