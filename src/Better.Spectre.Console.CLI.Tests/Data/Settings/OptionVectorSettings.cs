namespace Better.Spectre.Console.CLI.Tests.Data;

public class OptionVectorSettings : CommandSettings
{
    [CommandOption("--foo")]
    public string[] Foo { get; set; }

    [CommandOption("--bar")]
    public int[] Bar { get; set; }
}
