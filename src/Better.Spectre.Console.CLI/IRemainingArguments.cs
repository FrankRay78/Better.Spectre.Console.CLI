namespace Better.Spectre.Console.CLI;

/// <summary>
/// Represents the remaining arguments.
/// </summary>
public interface IRemainingArguments
{
    /// <summary>
    /// Gets the parsed remaining arguments.
    /// </summary>
    ILookup<string, string?> Parsed { get; }

    /// <summary>
    /// Gets the raw, non-parsed remaining arguments.
    /// This is normally everything after the `--` delimiter.
    /// </summary>
    IReadOnlyList<string> Raw { get; }
}