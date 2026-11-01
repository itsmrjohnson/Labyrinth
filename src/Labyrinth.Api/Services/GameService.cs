using Labyrinth.Domain;

namespace Labyrinth.Api.Services;

public sealed class GameService
{
    private readonly BoardGenerator _boardGenerator;
    private readonly GameStore _gameStore;

    public GameService(
        BoardGenerator boardGenerator,
        GameStore gameStore)
    {
        _boardGenerator = boardGenerator;
        _gameStore = gameStore;
    }

    public (Guid GameId, Game Game) CreateNewGame(int playerCount)
    {
        if (playerCount is < 2 or > 4)
        {
            throw new ArgumentOutOfRangeException(
                nameof(playerCount),
                "Player count must be between 2 and 4.");
        }

        var game = Game.CreateNew(
            _boardGenerator,
            playerCount);

        var gameId = _gameStore.AddGame(game);

        return (gameId, game);
    }

    public bool TryGetGame(Guid gameId, out Game? game)
    {
        return _gameStore.TryGetGame(gameId, out game);
    }


}