namespace Better.Spectre.Console.CLI.Tests.Data;

public sealed class GenericCommand<TSettings> : Command<TSettings>
    where TSettings : CommandSettings
{
    public override int Execute(CommandContext context, TSettings settings)
    {
        return 0;
    }
}
