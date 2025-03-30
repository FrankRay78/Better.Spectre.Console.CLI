using SystemConsole = System.Console;

namespace Better.Spectre.Console.CLI.Tests.Data;

public abstract class AnimalCommand<TSettings> : Command<TSettings>
    where TSettings : CommandSettings
{
}
