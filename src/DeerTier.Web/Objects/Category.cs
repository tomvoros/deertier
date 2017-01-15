using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeerTier.Web.Objects
{
    [Table("tblCategories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string UrlName { get; set; }
        public int? ParentId { get; set; }
        public int? SectionId { get; set; }
        public bool AllowSubmission { get; set; }
        public bool Visible { get; set; }
        public int DisplayOrder { get; set; }
        public bool GameTime { get; set; }
        public bool EscapeGameTime { get; set; }
        public bool RealTime { get; set; }
        public string ShortName { get; set; }
        public string WikiUrl { get; set; }

        [NotMapped]
        public Section Section { get; set; }

        [NotMapped]
        public Category Parent { get; set; }

        [NotMapped]
        public Category[] Subcategories { get; set; }

        [NotMapped]
        public Category DefaultSubcategory { get; set; }
    }
}