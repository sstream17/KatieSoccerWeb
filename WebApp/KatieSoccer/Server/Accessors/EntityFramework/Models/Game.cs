using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatieSoccer.Server.Accessors.EntityFramework.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "varchar(64)")]
        public string GameId { get; set; }

        [Column(TypeName = "int")]
        public int IsOnline { get; set; }

        public Player PlayerOne { get; set; }

        public Player PlayerTwo { get; set; }
    }
}
