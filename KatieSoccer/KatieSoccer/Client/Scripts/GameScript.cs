using System.Collections.Generic;
using KatieSoccer.Client.Models;

namespace KatieSoccer.Client.Scripts
{
    public class GameScript
    {
        public List<PieceModel> TeamOnePieces { get; set; }
        public List<PieceModel> TeamTwoPieces { get; set; }

        public GameScript()
        {
            TeamOnePieces = new List<PieceModel>
            {
                new PieceModel
                {
                    Position = new Position(100, 100),
                    Color = "#CC0000"
                },
                new PieceModel
                {
                    Position = new Position(300, 300),
                    Color = "#CC0000"
                },
                new PieceModel
                {
                    Position = new Position(500, 500),
                    Color = "#CC0000"
                }
            };

            TeamTwoPieces = new List<PieceModel>
            {
                new PieceModel
                {
                    Position = new Position(1100, 100),
                    Color = "#2499F2"
                },
                new PieceModel
                {
                    Position = new Position(900, 300),
                    Color = "#2499F2"
                },
                new PieceModel
                {
                    Position = new Position(700, 500),
                    Color = "#2499F2"
                }
            };
        }
    }
}
