namespace Better.Spectre.Console.CLI.Tests.Data;

public class MammalSettings : AnimalSettings
{
    [CommandOption("-n|-p|--name|--pet-name <VALUE>")]
    public string Name { get; set; }
}
