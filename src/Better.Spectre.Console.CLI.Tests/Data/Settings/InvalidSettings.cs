namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class InvalidSettings : CommandSettings
{
    [CommandOption("-f|--foo [BAR]")]
    public string Value { get; set; }
}
