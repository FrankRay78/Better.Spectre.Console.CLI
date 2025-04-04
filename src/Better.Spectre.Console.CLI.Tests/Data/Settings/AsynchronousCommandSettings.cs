namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class AsynchronousCommandSettings : CommandSettings
{
    [CommandOption("--ThrowException")]
    [DefaultValue(false)]
    public bool ThrowException { get; set; }
}