using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeerTier.Web.Objects
{
    [Table("tblUsers")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public PasswordType PasswordType { get; set; }
        public ModeratorType IsModerator { get; set; }
    }
}