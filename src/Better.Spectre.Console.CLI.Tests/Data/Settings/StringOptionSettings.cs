namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class StringOptionSettings : CommandSettings
{
    [CommandOption("-f|--foo")]
    public string Foo { get; set; }
}
