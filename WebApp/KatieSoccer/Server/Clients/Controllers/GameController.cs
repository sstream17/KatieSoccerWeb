﻿using System.Threading.Tasks;
using KatieSoccer.Server.Accessors;
using KatieSoccer.Shared;
using Microsoft.AspNetCore.Mvc;

namespace KatieSoccer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        public GameController(IGameAccessor gameAccessor)
        {
            GameAccessor = gameAccessor;
        }

        private IGameAccessor GameAccessor { get; }

        [HttpPost]
        public async Task<IActionResult> CreateGame(GameData gameData)
        {
            await GameAccessor.AddGame(gameData);
            return Created("/play", gameData);
        }

        [HttpPut]
        [Route("{gameId}")]
        public async Task<IActionResult> AddPlayer(string gameId, Player player)
        {
            var isPlayerOne = await GameAccessor.AddPlayer(gameId, player);
            return Ok(isPlayerOne);
        }
    }
}
