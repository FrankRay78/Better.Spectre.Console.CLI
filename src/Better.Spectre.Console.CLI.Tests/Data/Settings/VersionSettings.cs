namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class VersionSettings : CommandSettings
{
    [CommandOption("-v|--version")]
    [Description("The command version")]
    public string Version { get; set; }
}