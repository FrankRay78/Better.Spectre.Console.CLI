namespace Better.Spectre.Console.CLI;

[Description("Displays the CLI library version")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class VersionCommand : Command<VersionCommand.Settings>
{
    private readonly IAnsiConsole _writer;

    public VersionCommand(IConfiguration configuration)
    {
        _writer = configuration.Settings.Console.GetConsole();
    }

    public sealed class Settings : CommandSettings
    {
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        _writer.MarkupLine(
            "[yellow]Better.Spectre.Console.CLI[/] version [aqua]{0}[/]",
            VersionHelper.GetVersion(typeof(VersionCommand)?.Assembly));

        _writer.MarkupLine(
            "[yellow]Spectre.Console[/] version [aqua]{0}[/]",
            VersionHelper.GetVersion(typeof(IAnsiConsole)?.Assembly));

        return 0;
    }
}