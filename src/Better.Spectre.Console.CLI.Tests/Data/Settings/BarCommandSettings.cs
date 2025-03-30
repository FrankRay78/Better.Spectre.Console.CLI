namespace Better.Spectre.Console.CLI.Tests.Data;

public class BarCommandSettings : FooCommandSettings
{
    [CommandArgument(0, "<CORGI>")]
    [Description("The corgi value.")]
    public string Corgi { get; set; }
}
