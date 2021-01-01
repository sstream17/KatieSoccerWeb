using System.ComponentModel.DataAnnotations;

namespace KatieSoccer.Server.Accessors.EntityFramework.Models
{
    public class Game
    {
        [Key]
        public string GameId { get; set; }

        public int IsOnline { get; set; }

        public Player PlayerOne { get; set; }

        public Player PlayerTwo { get; set; }
    }
}
