using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KatieSoccer.Client.Models;

namespace KatieSoccer.Client.Scripts
{
    public class GameScript
    {
        public event EventHandler OnUpdate;

        public List<PieceModel> TeamOnePieces { get; set; }
        public List<PieceModel> TeamTwoPieces { get; set; }

        public GameScript()
        {
            TeamOnePieces = new List<PieceModel>
            {
                new PieceModel
                {
                    Position = new Vector2(100, 100),
                    Color = "#CC0000"
                },
                new PieceModel
                {
                    Position = new Vector2(300, 300),
                    Color = "#CC0000"
                },
                new PieceModel
                {
                    Position = new Vector2(500, 500),
                    Color = "#CC0000"
                }
            };

            TeamTwoPieces = new List<PieceModel>
            {
                new PieceModel
                {
                    Position = new Vector2(1100, 100),
                    Color = "#2499F2"
                },
                new PieceModel
                {
                    Position = new Vector2(900, 300),
                    Color = "#2499F2"
                },
                new PieceModel
                {
                    Position = new Vector2(700, 500),
                    Color = "#2499F2"
                }
            };
        }

        public async Task Update()
        {
            while (true)
            {
                OnUpdate?.Invoke(this, EventArgs.Empty);
                await Task.Delay(10);
            }
        }
    }
}
