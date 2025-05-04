namespace Audora.Application.Common.Results;

public readonly record struct Error(string Code, string Description, ErrorType Type)
{
    public static Error NoErrors =>
        Unexpected("Result.NoErrors", "Error list cannot be retrieved from a successful Result.");
    
    
    public static Error Failure(string code = "General.Failure", string description = "A 'failure' error has occurred.")
    {
        return new Error(code, description, ErrorType.Failure);
    }

    public static Error Validation(string code = "General.Validation",
        string description = "A 'validation' error has occurred.")
    {
        return new Error(code, description, ErrorType.Validation);
    }

    public static Error Conflict(string code = "General.Conflict",
        string description = "A 'conflict' error has occurred.")
    {
        return new Error(code, description, ErrorType.Conflict);
    }

    public static Error NotFound(string code = "General.NotFound",
        string description = "A 'Not Found' error has occurred.")
    {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error Unauthorized(string code = "General.Unauthorized",
        string description = "An 'Unauthorized' error has occurred.")
    {
        return new Error(code, description, ErrorType.Unauthorized);
    }
    
    public static Error Unexpected(string code = "General.Unexpected",
        string description = "An 'unexpected' error has occurred.")
    {
        return new Error(code, description, ErrorType.Unexpected);
    }

    public override string ToString() => $"{Code}: {Description}";
}