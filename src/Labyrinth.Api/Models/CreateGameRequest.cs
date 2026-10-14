using System.ComponentModel.DataAnnotations;

namespace Labyrinth.Api.Models;

public sealed class CreateGameRequest
{
    [Range(2, 4)]
    public int PlayerCount { get; init; }
}