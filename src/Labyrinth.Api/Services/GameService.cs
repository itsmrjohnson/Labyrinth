using Labyrinth.Domain;

namespace Labyrinth.Api.Services;

public sealed class GameService
{
    private readonly BoardGenerator _boardGenerator;

    public GameService(BoardGenerator boardGenerator)
    {
        _boardGenerator = boardGenerator;
    }

    public Game CreateNewGame(int playerCount)
    {
        if (playerCount is < 2 or > 4)
        {
            throw new ArgumentOutOfRangeException(
                nameof(playerCount),
                "Player count must be between 2 and 4.");
        }

        return Game.CreateNew(
            _boardGenerator,
            playerCount);
    }
}