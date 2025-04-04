namespace Better.Spectre.Console.CLI;

/// <summary>
/// Represents a command context.
/// </summary>
public sealed class CommandContext
{
    /// <summary>
    /// Gets the remaining arguments.
    /// </summary>
    /// <value>
    /// The remaining arguments.
    /// </value>
    public IRemainingArguments Remaining { get; }

    /// <summary>
    /// Gets all the arguments that were passed to the application.
    /// </summary>
    public IReadOnlyList<string> Arguments { get; }

    /// <summary>
    /// Gets the name of the command.
    /// </summary>
    /// <value>
    /// The name of the command.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Gets the data that was passed to the command during registration (if any).
    /// </summary>
    /// <value>
    /// The command data.
    /// </value>
    public object? Data { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandContext"/> class.
    /// </summary>
    /// <param name="arguments">All arguments that were passed to the application.</param>
    /// <param name="remaining">The remaining arguments.</param>
    /// <param name="name">The command name.</param>
    /// <param name="data">The command data.</param>
    public CommandContext(
        IEnumerable<string> arguments,
        IRemainingArguments remaining,
        string name,
        object? data)
    {
        Arguments = arguments.ToSafeReadOnlyList();
        Remaining = remaining ?? throw new System.ArgumentNullException(nameof(remaining));
        Name = name ?? throw new System.ArgumentNullException(nameof(name));
        Data = data;
    }
}