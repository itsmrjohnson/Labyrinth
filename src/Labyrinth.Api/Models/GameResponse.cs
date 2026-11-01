
using Labyrinth.Domain;

namespace Labyrinth.Api.Models;

public sealed class GameResponse
{
    public Guid GameId { get; init; }

    public int CurrentPlayerIndex { get; init; }

    public GameTurnPhase TurnPhase { get; init; }

    public IReadOnlyList<Position> PlayerPositions { get; init; } = [];

    public IReadOnlyList<GameTileResponse> Tiles { get; init; } = [];

    public GameTileResponse SpareTile { get; init; } = new();
}
