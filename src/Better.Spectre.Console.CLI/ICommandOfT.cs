namespace Better.Spectre.Console.CLI;

/// <summary>
/// Represents a command.
/// </summary>
/// <typeparam name="TSettings">The settings type.</typeparam>
public interface ICommand<TSettings> : ICommandLimiter<TSettings>
    where TSettings : CommandSettings
{
    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="context">The command context.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>An integer indicating whether or not the command executed successfully.</returns>
    Task<int> Execute(CommandContext context, TSettings settings);
}