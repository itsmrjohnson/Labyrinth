using Labyrinth.Api.Models;
using Labyrinth.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Labyrinth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class GamesController : ControllerBase
{
    private readonly GameService _gameService;

    public GamesController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpPost]
    public ActionResult<CreateGameResponse> CreateGame(
        CreateGameRequest request)
    {
        var game = _gameService.CreateNewGame(
            request.PlayerCount);

        var response = new CreateGameResponse
        {
            CurrentPlayerIndex = game.CurrentPlayerIndex,
            TurnPhase = game.TurnPhase,
            PlayerPositions = game.Players
                .Select(player => player.Position)
                .ToList()
        };

        return Ok(response);
    }
}