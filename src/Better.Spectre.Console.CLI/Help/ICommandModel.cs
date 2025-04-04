namespace Better.Spectre.Console.CLI.Help;

/// <summary>
/// Represents a command model.
/// </summary>
public interface ICommandModel : ICommandContainer
{
    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    string ApplicationName { get; }

    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    string? ApplicationVersion { get; }
}
