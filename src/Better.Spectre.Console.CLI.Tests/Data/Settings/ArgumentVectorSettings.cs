namespace Better.Spectre.Console.CLI.Tests.Data;

public class ArgumentVectorSettings : CommandSettings
{
    [CommandArgument(0, "<Foos>")]
    public string[] Foo { get; set; }
}
