
namespace Labyrinth.Api.Models;

public sealed class GameTileResponse
{
    public int Row { get; init; }

    public int Column { get; init; }

    public string Type { get; init; } = string.Empty;

    public string Rotation { get; init; } = string.Empty;

    public string Movement { get; init; } = string.Empty;

    public IReadOnlyList<string> Connections { get; init; } = [];
}
