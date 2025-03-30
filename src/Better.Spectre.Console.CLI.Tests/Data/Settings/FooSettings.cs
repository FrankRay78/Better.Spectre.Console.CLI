namespace Better.Spectre.Console.CLI.Tests.Data;

public class FooCommandSettings : CommandSettings
{
    [CommandArgument(0, "[QUX]")]
    [Description("The qux value.")]
    public string Qux { get; set; }
}
