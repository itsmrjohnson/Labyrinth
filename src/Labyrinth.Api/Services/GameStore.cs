using System.Collections.Concurrent;
using Labyrinth.Domain;

namespace Labyrinth.Api.Services;

public sealed class GameStore
{
    private readonly ConcurrentDictionary<Guid, Game> _games = new();

    public Guid AddGame(Game game)
    {
        var gameId = Guid.NewGuid();

        _games.TryAdd(gameId, game);

        return gameId;
    }

    public bool TryGetGame(Guid gameId, out Game? game)
    {
        return _games.TryGetValue(gameId, out game);
    }
}