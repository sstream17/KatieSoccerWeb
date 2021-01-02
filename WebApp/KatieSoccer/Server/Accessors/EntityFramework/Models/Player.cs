using Microsoft.EntityFrameworkCore;

namespace KatieSoccer.Server.Accessors.EntityFramework.Models
{
    [Owned]
    public class Player
    {
        public bool IsLocal { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
