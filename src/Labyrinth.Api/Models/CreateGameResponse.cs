using Labyrinth.Domain;

namespace Labyrinth.Api.Models;

public sealed class CreateGameResponse
{
    public int CurrentPlayerIndex { get; init; }

    public GameTurnPhase TurnPhase { get; init; }

    public IReadOnlyList<Position> PlayerPositions { get; init; } = [];
}