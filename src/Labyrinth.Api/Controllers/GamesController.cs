
using Labyrinth.Api.Mappings;
using Labyrinth.Api.Models;
using Labyrinth.Api.Services;
using Labyrinth.Domain;
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
    public ActionResult<GameResponse> CreateGame(
        CreateGameRequest request)
    {
        var (gameId, game) = _gameService.CreateNewGame(request.PlayerCount);

        var tiles = GameBoardMapper.ToGameTileResponses(game.Board);

        var response = new GameResponse
        {
            GameId = gameId,
            CurrentPlayerIndex = game.CurrentPlayerIndex,
            TurnPhase = game.TurnPhase,
            PlayerPositions = game.Players
                .Select(player => player.Position)
                .ToList(),
            Tiles = tiles,
            SpareTile = GameTileMapper.ToGameTileResponse(
                game.Board.SpareTile)
        };

        return Ok(response);
    }

    [HttpGet("{gameId:guid}")]
    public ActionResult<GameResponse> GetGame(Guid gameId)
    {
        if (!_gameService.TryGetGame(gameId, out var game) || game is null)
        {
            return NotFound();
        }

        var tiles = GameBoardMapper.ToGameTileResponses(game.Board);

        var response = new GameResponse
        {
            GameId = gameId,
            CurrentPlayerIndex = game.CurrentPlayerIndex,
            TurnPhase = game.TurnPhase,
            PlayerPositions = game.Players
                .Select(player => player.Position)
                .ToList(),
            Tiles = tiles,
            SpareTile = GameTileMapper.ToGameTileResponse(game.Board.SpareTile)
        };

        return Ok(response);
    }

    [HttpPost("{gameId:guid}/insert")]
    public ActionResult<GameResponse> InsertSpareTile(
    Guid gameId,
    InsertSpareTileRequest request)
    {
        if (!_gameService.TryGetGame(gameId, out var game) || game is null)
        {
            return NotFound();
        }

        game.InsertSpareTile(
            request.Location,
            request.Index,
            request.Rotation);

        var response = new GameResponse
        {
            GameId = gameId,
            CurrentPlayerIndex = game.CurrentPlayerIndex,
            TurnPhase = game.TurnPhase,
            PlayerPositions = game.Players
                .Select(player => player.Position)
                .ToList(),
            Tiles = GameBoardMapper.ToGameTileResponses(game.Board),
            SpareTile = GameTileMapper.ToGameTileResponse(game.Board.SpareTile)
        };

        return Ok(response);
    }
}
