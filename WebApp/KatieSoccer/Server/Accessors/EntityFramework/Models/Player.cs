using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatieSoccer.Server.Accessors.EntityFramework.Models
{
    public class Player
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "int")]
        public int IsLocal { get; set; }

        [Column(TypeName = "varchar(32)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(16)")]
        public string Color { get; set; }
    }
}
