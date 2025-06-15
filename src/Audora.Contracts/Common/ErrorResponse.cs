namespace Audora.Contracts.Common;

public class ErrorResponse
{
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;
}